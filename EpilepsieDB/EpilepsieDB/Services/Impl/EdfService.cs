using EpilepsieDB.EDF;
using EpilepsieDB.Models;
using System.Collections.Generic;
using System.Globalization;
using System;
using System.Linq;
using System.Collections;
using EpilepsieDB.Source.Helper;

namespace EpilepsieDB.Services.Impl
{
    public class TimeInfo
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public float Gap { get; set; }
    }


    public class EdfService : IEdfService
    {
        public EdfFile ReadFile(string path)
        {
            EdfFile file;

            using (var reader = new EdfReader(path))
            {
                if (reader.BaseStream.Length < 256)
                {
                    Console.WriteLine("Error: File is not a valid EDF-File");
                    return null;
                }

                try
                {
                    EdfHeader header = reader.ReadHeader();
                    EdfSignal[] signals = new EdfSignal[0]; // reader.ReadSignals(header);
                    EdfAnnotation[] annotations = reader.ReadAnnotations(header);
                    float[] blockOffsets = GetOffsets(annotations);
                    file = new EdfFile(header, blockOffsets, signals, annotations);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Something went wrong parsing the file!");
                    Console.WriteLine(e.Message);
                    Console.WriteLine(e.StackTrace);
                    return null;
                }
            }

            return file;
        }

        public bool WriteToScan(Scan scan, string path)
        {
            EdfFile file = ReadFile(path);

            if (file == null)
                return false;

            PopulateScanWithHeaderInfo(scan, file.Header);
            PopulateScanWithBlockInfos(scan, file.Header, file.BlockOffsets);
            PopulateScanWithNonTimeKeepingAnnotations(scan, file.Annotations);
            PopulateScanWithSignalInfos(scan, file.Header.SignalHeaders);

            return true;
        }

        private float[] GetOffsets(EdfAnnotation[] annotations)
        {
            return annotations.Where(x => x.IsOnset).Select(x => x.StartOffset).OrderBy(x => x).ToArray();
        }

        private void PopulateScanWithHeaderInfo(Scan scan, EdfHeader header)
        {
            CultureInfo provider = CultureInfo.GetCultureInfo("de-DE");

            scan.Version = header.Version.Value;
            scan.PatientInfo = header.PatientID.Value;
            scan.RecordInfo = header.RecordID.Value;
            scan.StartDate = DateTime.ParseExact(header.StartDate.Value, "dd.MM.yyyy", provider);
            scan.StartDate = DateTime.SpecifyKind(scan.StartDate, DateTimeKind.Utc);
            scan.StartTime = DateTime.ParseExact(header.StartTime.Value, "HH.mm.ss", provider);
            scan.StartTime = DateTime.SpecifyKind(scan.StartTime, DateTimeKind.Utc);
            scan.NumberOfRecords = header.NumberOfDataRecords.Value;
            scan.DurationOfDataRecord = header.DurationOfDataRecord.Value;
            scan.NumberOfSignals = header.NumberOfSignals.Value;
        }

        private void PopulateScanWithBlockInfos(Scan scan, EdfHeader header, float[] blockOffsets)
        {
            List<Block> blocks = new List<Block>();

            DateTime starttime = new DateTime(
                            scan.StartDate.Year,
                            scan.StartDate.Month,
                            scan.StartDate.Day,
                            scan.StartTime.Hour,
                            scan.StartTime.Minute,
                            scan.StartTime.Second,
                            scan.StartTime.Millisecond);
            DateTime endTime = starttime.AddSeconds(header.DurationOfDataRecord.Value);

            BlockTimeIterator iterator = new BlockTimeIterator(starttime, endTime, header.DurationOfDataRecord.Value, header.IsInterupted);

            foreach (var offset in blockOffsets)
            {
                TimeInfo timeInfo = iterator.Next(offset);

                Block block = new Block()
                {
                    Scan = scan,
                    Starttime = timeInfo.StartTime,
                    Endtime = timeInfo.EndTime,
                    GapToPrevious = timeInfo.Gap,
                };

                blocks.Add(block);
            }

            scan.Blocks = blocks;
        }

        private void PopulateScanWithSignalInfos(Scan scan, EdfSignalHeader[] signals)
        {
            List<string> labels = new List<string>();
            List<string> transducerTypes = new List<string>();
            List<string> dimensions = new List<string>();

            foreach (var signal in signals)
            {
                labels.Add(signal.Label.Value);
                transducerTypes.Add(signal.TransducerType.Value);
                dimensions.Add(signal.PhysicalDimension.Value);
            }

            // add fields distinct for searching
            labels = labels.Distinct().ToList();
            transducerTypes = transducerTypes.Distinct().ToList();
            dimensions = dimensions.Distinct().ToList();

            scan.Labels = String.Join(";", labels.ToArray());
            scan.TransducerTypes = String.Join(";", transducerTypes.ToArray());
            scan.PhysicalDimensions = String.Join(";", dimensions.ToArray());

            // set annotations
            scan.Signals = ConstructSignals(scan, signals);
        }

        private List<Signal> ConstructSignals(Scan scan, EdfSignalHeader[] data)
        {
            List<Signal> signals = new List<Signal>();

            for (int i = 0; i < data.Length; i++)
            {
                Signal signal = new Signal()
                {
                    ScanID = scan.ID,
                    Scan = scan,
                    Channel = i,
                    Label = data[i].Label.Value,
                    TransducerType = data[i].TransducerType.Value,
                    PhysicalDimension = data[i].PhysicalDimension.Value,
                    PhysicalMinimum = data[i].PhysicalMinimum.Value,
                    PhysicalMaximum = data[i].PhysicalMaximum.Value,
                    DigitalMinimum = data[i].DigitalMinimum.Value,
                    DigitalMaximum = data[i].DigitalMaximum.Value,
                    Prefiltering = data[i].Prefiltering.Value,
                    Reserved = data[i].Reserved.Value,
                    NumberOfSamples = data[i].NumberOfSamples.Value
                };

                signals.Add(signal);
            }

            return signals;
        }

        private void PopulateScanWithNonTimeKeepingAnnotations(Scan scan, EdfAnnotation[] data)
        {
            data = data.Where(x => !x.IsOnset).ToArray();
            scan.Annotations = GetAnnotationsFromEdfAnnotations(scan, data);
        }

        private List<Annotation> GetAnnotationsFromEdfAnnotations(Scan scan, EdfAnnotation[] data)
        {
            List<Annotation> annotations = new List<Annotation>();

            foreach (var entry in data)
            {
                Annotation annotation = new Annotation()
                {
                    Scan = scan,
                    Offset = entry.StartOffset,
                    Duration = entry.Duration,
                    Description = entry.Description
                };

                annotations.Add(annotation);
            }

            return annotations;
        }
    }
}
