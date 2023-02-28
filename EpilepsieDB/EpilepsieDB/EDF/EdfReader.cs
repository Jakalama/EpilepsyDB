using EpilepsieDB.Source.Helper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace EpilepsieDB.EDF
{
    /// <summary>
    /// This EDF-File Reader is strongly inspired by this project: https://github.com/rdtek/LibEDF 
    /// Currently (15.12.22) the above mentioned project is under the MIT License.
    /// </summary>
    public class EdfReader : BinaryReader
    {
        private byte[] buffer;
        private int bufferOffset;

        public EdfReader(string filepath) : base(new FileStream(filepath, FileMode.Open)) { }

        public EdfHeader ReadHeader()
        {
            this.BaseStream.Seek(0, SeekOrigin.Begin);

            EdfHeader header = new EdfHeader();

            bufferOffset = 0;
            buffer = this.ReadBytes(256);

            header.Version.Value = ReadAsciiFromBuffer(EdfField.Version);
            header.PatientID.Value = ReadAsciiFromBuffer(EdfField.PatientID);
            header.RecordID.Value = ReadAsciiFromBuffer(EdfField.RecordID);
            header.StartDate.Value = ReadAsciiFromBuffer(EdfField.StartDate);

            string date = header.StartDate.Value;
            string yearStr = date.Substring(date.Length - 2, 2);
            int year;
            if (Int32.TryParse(yearStr, out year))
            {
                if (year <= 99 && year >= 85)
                    year += 1900;
                else
                    year += 2000;
            }

            header.StartDate.Value = date.Substring(0, 6) + year.ToString();

            header.StartTime.Value = ReadAsciiFromBuffer(EdfField.StartTime);
            header.NumberOfBytesInHeader.Value = ReadInt16FromBuffer(EdfField.NumberOfBytesInHeader);
            header.Reserved.Value = ReadAsciiFromBuffer(EdfField.Reserved);

            string reserved = header.Reserved.Value.ToLower();
            if (reserved.StartsWith("edf+d"))
            {
                header.IsEdfPlus = true;
                header.IsInterupted = true;
            }
            else if (reserved.StartsWith("edf+c"))
            {
                header.IsEdfPlus = true;
                header.IsInterupted = false;
            }
            else
            {
                header.IsEdfPlus = false;
            }

            header.NumberOfDataRecords.Value = ReadInt16FromBuffer(EdfField.NumberOfDataRecords);

            if (header.NumberOfDataRecords.Value == -1)
                Console.WriteLine("File is not valid! Invalid amount of data records");

            header.DurationOfDataRecord.Value = (float)ReadDoubleFromBuffer(EdfField.DurationOfDataRecord);
            header.NumberOfSignals.Value = ReadInt16FromBuffer(EdfField.NumberOfSignals);

            // this section handles cases where edf+ format needs special attention
            // eg. recording dates after 2084 or before 1985
            //  0123456789 123456789 123456789 123456789 123456789 123456789 123456789 123456789 
            // "Startdate 12-AUG-2099 X X BCI2000                                               "
            if (header.IsEdfPlus)
            {
                // - patient info
                string patientInfo = header.RecordID.Value.ToLower().Trim();
                string[] subFields = patientInfo.Split(" ");

                if (subFields.Length < 4)
                    Console.WriteLine("Error: officially not an edf+ file!");


                // - record info
                string recordInfo = header.RecordID.Value.ToLower().Trim();
                subFields = recordInfo.Split(" ");

                if (subFields.Length < 5)
                    Console.WriteLine("Warning: officially not an edf+ file!");

                // -- date
                string dateField = subFields[1];
                if (dateField != "x")
                {
                    DateTime startDate = DateTime.ParseExact(dateField, "dd-MMM-yyyy", CultureInfo.GetCultureInfo("de-DE"));
                    string dateStr = startDate.ToString("dd.MM.yyyy");

                    if (dateStr.ToLower() != header.StartDate.Value)
                        Console.WriteLine("Warning: Date field and record info don't match!");

                    header.StartDate.Value = dateStr;
                }

            }

            ReadSignalInfos(header);

            return header;
        }

        public EdfAnnotation[] ReadAnnotations(EdfHeader header)
        {
            List<EdfAnnotation> annotations = new List<EdfAnnotation>();

            // read annotations for each block
            int readPosition = header.NumberOfBytesInHeader.Value;

            int dataRecordSize = 0;
            List<SignalSection> annotationSections = new List<SignalSection>();

            // get positions of all annotation signals inside the data records
            // calculate data record size
            for (int i = 0; i < header.SignalHeaders.Length; i++)
            {
                int numSamples = header.SignalHeaders[i].NumberOfSamples.Value * 2;

                if (header.SignalHeaders[i].Label.Value.ToLower().Contains("annotations"))
                {
                    annotationSections.Add(new SignalSection()
                    {
                        Offset = dataRecordSize,
                        Length = numSamples
                    });
                }

                dataRecordSize += numSamples;
            }

            for (int i = 0; i < header.NumberOfDataRecords.Value; i++)
            {
                // setup read to buffer
                bufferOffset = 0;
                this.BaseStream.Seek(readPosition, SeekOrigin.Begin);
                buffer = this.ReadBytes(dataRecordSize);

                // increase next read pos
                readPosition += dataRecordSize;

                // get annotations
                foreach (SignalSection section in annotationSections)
                {
                    char[] chars = Encoding.ASCII.GetChars(buffer, section.Offset, section.Length);
                    annotations.AddRange(ConstructAnnotation(chars));
                }
            }

            return annotations.ToArray();
        }

        public EdfSignal[] ReadSignals(EdfHeader header)
        {
            EdfSignal[] signals = new EdfSignal[header.SignalHeaders.Length];

            // read annotations for each block
            int readPosition = header.NumberOfBytesInHeader.Value;

            int dataRecordSize = 0;
            List<SignalSection> signalSection = new List<SignalSection>();

            // get positions of all signals inside the data records
            // calculate data record size
            int numSignals = 0;
            for (int i = 0; i < header.SignalHeaders.Length; i++)
            {
                int numSamples = header.SignalHeaders[i].NumberOfSamples.Value * 2;

                if (!header.SignalHeaders[i].Label.Value.ToLower().Contains("annotations"))
                {
                    signalSection.Add(new SignalSection()
                    {
                        SignalIndex = numSignals++,
                        Offset = dataRecordSize,
                        Length = numSamples
                    });
                }

                dataRecordSize += numSamples;
            }

            for (int i = 0; i < header.NumberOfDataRecords.Value; i++)
            {
                // setup read to buffer
                bufferOffset = 0;
                this.BaseStream.Seek(readPosition, SeekOrigin.Begin);
                buffer = this.ReadBytes(dataRecordSize);

                // increase next read pos
                readPosition += dataRecordSize;

                // get annotations
                foreach (SignalSection section in signalSection)
                {
                    short[] samples = new short[(int)Math.Ceiling((decimal)section.Length)];
                    Buffer.BlockCopy(buffer, section.Offset, samples, 0, section.Length);

                    if (signals[section.SignalIndex] == null)
                        signals[section.SignalIndex] = new EdfSignal();

                    signals[section.SignalIndex].Samples.AddRange(samples);
                }
            }

            return signals;
        }

        private void ReadSignalInfos(EdfHeader header)
        {
            int numberOfSignals = header.NumberOfSignals.Value;

            bufferOffset = 0;
            buffer = this.ReadBytes(256 * numberOfSignals);

            // first read the data out of the file as bulk
            string[] labels = ReadMultipleAsciisFromBuffer(EdfField.Labels, numberOfSignals);
            string[] transducerTypes = ReadMultipleAsciisFromBuffer(EdfField.TransducerType, numberOfSignals);
            string[] physicalDimensions = ReadMultipleAsciisFromBuffer(EdfField.PhysicalDimension, numberOfSignals);
            double[] physicalMinima = ReadMultipleDoubleFromBuffer(EdfField.PhysicalMinimum, numberOfSignals);
            double[] physicalMaxima = ReadMultipleDoubleFromBuffer(EdfField.PhysicalMaximum, numberOfSignals);
            short[] digitalMinima = ReadMultipleInt16FromBuffer(EdfField.DigitalMinimum, numberOfSignals);
            short[] digitalMaxima = ReadMultipleInt16FromBuffer(EdfField.DigitalMaximum, numberOfSignals);
            string[] prefilter = ReadMultipleAsciisFromBuffer(EdfField.Prefiltering, numberOfSignals);
            short[] numberOfSamples = ReadMultipleInt16FromBuffer(EdfField.NumberOfSamplesInDataRecord, numberOfSignals);
            string[] signalsReserved = ReadMultipleAsciisFromBuffer(EdfField.SignalsReserved, numberOfSignals);

            header.SignalHeaders = new EdfSignalHeader[numberOfSignals];

            // assign each signals data
            for (int i = 0; i < numberOfSignals; i++)
            {
                header.SignalHeaders[i] = new EdfSignalHeader();

                header.SignalHeaders[i].Label.Value = labels[i];
                header.SignalHeaders[i].TransducerType.Value = transducerTypes[i];
                header.SignalHeaders[i].PhysicalDimension.Value = physicalDimensions[i];
                header.SignalHeaders[i].PhysicalMinimum.Value = physicalMinima[i];
                header.SignalHeaders[i].PhysicalMaximum.Value = physicalMaxima[i];
                header.SignalHeaders[i].DigitalMinimum.Value = digitalMinima[i];
                header.SignalHeaders[i].DigitalMaximum.Value = digitalMaxima[i];
                header.SignalHeaders[i].Prefiltering.Value = prefilter[i];
                header.SignalHeaders[i].NumberOfSamples.Value = numberOfSamples[i];
                header.SignalHeaders[i].Reserved.Value = signalsReserved[i];
            }
        }

        private string ReadAsciiFromBuffer(EdfField field)
        {
            int valueSize = EdfFields.Map[field];
            byte[] bytes = buffer.SubArray(bufferOffset, valueSize);
            bufferOffset += valueSize;
            return Encoding.ASCII.GetString(bytes);
        }

        private short ReadInt16FromBuffer(EdfField field)
        {
            int valueSize = EdfFields.Map[field];
            byte[] bytes = buffer.SubArray(bufferOffset, valueSize);
            bufferOffset += valueSize;
            string intStr = Encoding.ASCII.GetString(bytes);
            short intRes = -1;

            if (!Int16.TryParse(intStr, out intRes))
                Console.WriteLine("Parsing of " + field.ToString() + " failed!");

            return intRes;
        }

        private double ReadDoubleFromBuffer(EdfField field)
        {
            int valueSize = EdfFields.Map[field];
            byte[] bytes = buffer.SubArray(bufferOffset, valueSize);
            bufferOffset += valueSize;
            string doubleStr = Encoding.ASCII.GetString(bytes);
            double doubleRes = -1;

            doubleStr = doubleStr.Replace(",", ".");
            if (!Double.TryParse(doubleStr, NumberStyles.Any, CultureInfo.InvariantCulture, out doubleRes))
                Console.WriteLine("Parsing of " + field.ToString() + " failed!");

            return doubleRes;
        }

        private string[] ReadMultipleAsciisFromBuffer(EdfField field, int count)
        {
            string[] list = new string[count];

            for (int i = 0; i < count; i++)
            {
                list[i] = ReadAsciiFromBuffer(field);
            }

            return list;
        }

        private short[] ReadMultipleInt16FromBuffer(EdfField field, int count)
        {
            short[] list = new short[count];

            for (int i = 0; i < count; i++)
            {
                list[i] = ReadInt16FromBuffer(field);
            }

            return list;
        }

        private double[] ReadMultipleDoubleFromBuffer(EdfField field, int count)
        {
            double[] list = new double[count];

            for (int i = 0; i < count; i++)
            {
                list[i] = ReadDoubleFromBuffer(field);
            }

            return list;
        }

        private IEnumerable<EdfAnnotation> ConstructAnnotation(char[] samples)
        {
            List<EdfAnnotation> annotations = new List<EdfAnnotation>();

            string data = new string(samples);
            data = data.Trim((char)0);

            if (String.IsNullOrEmpty(data))
                return annotations;

            // split at annotation seperator
            string[] dataAnnotations = data.Split((char)0);

            for (int i = 0; i < dataAnnotations.Length; i++)
            {
                if (String.IsNullOrEmpty(dataAnnotations[i]))
                    continue;

                EdfAnnotation annotation = new EdfAnnotation();

                // split at entry seperator
                string[] entries = dataAnnotations[i].Split((char)20);

                // get duration inside first entry
                string[] offsetAndDuration = entries[0].Split((char)21);
                float duration = 0f;
                float offset = 0f;

                if (offsetAndDuration.Length == 2)
                {
                    float.TryParse(offsetAndDuration[1], NumberStyles.Any, CultureInfo.InvariantCulture, out duration);
                }

                float.TryParse(offsetAndDuration[0], NumberStyles.Any, CultureInfo.InvariantCulture, out offset);

                string descriptions = "";

                // don't include entry containing the time information
                for (int j = 1; j < entries.Length; j++)
                {
                    if (!String.IsNullOrEmpty(entries[j]))
                        descriptions += entries[j].Trim() + "; ";
                }

                if (i == 0)
                    annotation.IsOnset = true;
                else
                    annotation.IsOnset = false;

                annotation.StartOffset = offset;
                annotation.Duration = duration;
                annotation.Description = descriptions.Trim(' ').Trim(';');

                annotations.Add(annotation);
            }

            return annotations;
        }

        private class SignalSection
        {
            public int SignalIndex { get; set; }
            public int Offset { get; set; }
            public int Length { get; set; }
        }
    }
}