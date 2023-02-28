using EpilepsieDB.Models;
using EpilepsieDB.Source.Helper;
using EpilepsieDB.Web.View.ViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using static System.Reflection.Metadata.BlobBuilder;

namespace EpilepsieDB.Web.View.Extensions
{
    public static class ScanExtension
    {
        public static ScanDto ToDto(this Scan scan)
        {
            ScanDto dto = new ScanDto()
            {
                ScanID = scan.ID,
                ScanNumber = scan.ScanNumber,
                RecordingID = scan.RecordingID,
                PatientAcronym = scan.Recording?.Patient?.Acronym
            };

            return dto;
        }

        public static ScanDetailDto ToDetailDto(this Scan scan, IEnumerable<Signal> signals = null, IEnumerable<Block> blocks = null)
        {
            ScanDetailDto dto = new ScanDetailDto()
            {
                ScanID = scan.ID,
                ScanNumber = scan.ScanNumber,
                RecordingNumber = scan.Recording?.RecordingNumber,
                RecordingID = scan.RecordingID,
                PatientAcronym = scan.Recording?.Patient?.Acronym,
                Version = scan.Version,
                PatientInfo = scan.PatientInfo,
                RecordInfo = scan.RecordInfo,
                StartDate = new DateTime(scan.StartDate.Year, scan.StartDate.Month, scan.StartDate.Day, scan.StartTime.Hour, scan.StartTime.Minute, scan.StartTime.Second, scan.StartTime.Millisecond),
                NumberOfRecords = scan.NumberOfRecords,
                DurationOfDataRecord = scan.DurationOfDataRecord,
                NumberOfSignals = scan.NumberOfSignals,
                Labels = scan.Labels,
                TransducerTypes = scan.TransducerTypes,
                PhysicalDimensions = scan.PhysicalDimensions
            };

            if (signals != null)
                dto.Electrodes = GetElectrodeDetails(scan.DurationOfDataRecord, signals);
            if (signals != null && blocks != null)
                dto.Blocks = GetBlockDetails(scan.DurationOfDataRecord, dto.Electrodes.Count(), blocks);

            return dto;
        }

        public static IEnumerable<ScanDto> ToDto(this IEnumerable<Scan> scans)
        {
            List<ScanDto> dtos = new List<ScanDto>();

            foreach (Scan scan in scans)
            {
                dtos.Add(scan.ToDto());
            }

            return dtos;
        }

        public static Scan ToModel(this ScanDto dto, Recording recording = null)
        {
            Scan model = new Scan()
            {
                ID = dto.ScanID,
                ScanNumber = dto.ScanNumber,
                RecordingID = dto.RecordingID,
                Recording = recording
            };

            return model;
        }

        private static IEnumerable<ElectrodeDetail> GetElectrodeDetails(float blockDuration, IEnumerable<Signal> signals)
        {
            List<ElectrodeDetail> list = new List<ElectrodeDetail>();

            foreach (Signal signal in signals)
            {
                if (signal.Label.ToLower().Contains("edf annotations"))
                    continue;

                ElectrodeDetail detail = new ElectrodeDetail()
                {
                    ElectrodeNumber = signal.Channel + 1,
                    Label = signal.Label,
                    Type = signal.TransducerType,

                    Gain = Calculator.Gain(
                        (float) signal.PhysicalMaximum, 
                        (float) signal.PhysicalMinimum,
                        signal.DigitalMaximum,
                        signal.DigitalMinimum),
                    Samples = signal.NumberOfSamples,
                    SamplingRate = Calculator.SamplingRate(
                        blockDuration,
                        signal.NumberOfSamples)
                };

                list.Add(detail);
            }

            return list;
        }

        private static IEnumerable<BlockDetail> GetBlockDetails(float blockDuration, int numChannels, IEnumerable<Block> blocks)
        {
            blocks = blocks.OrderBy(b => b.Starttime);
            List<BlockDetail> list = new List<BlockDetail>();

            int i = 1;
            foreach (Block block in blocks)
            {
                BlockDetail detail = new BlockDetail()
                {
                    BlockNumber = i,
                    NumChannels = numChannels,
                    Duration = blockDuration,
                    Starttime = block.Starttime,
                    Endtime = block.Endtime,
                    Gap = (int) block.GapToPrevious
                };

                i += 1;
                list.Add(detail);
            }

            return list;
        }
    }
}
