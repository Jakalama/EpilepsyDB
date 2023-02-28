using EpilepsieDB.Models;
using EpilepsieDB.Repositories;
using Microsoft.AspNetCore.Razor.Language;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace EpilepsieDB.Services.Impl
{
    public class SearchService : ISearchService
    {
        private readonly IRepository<Scan> _scanRepository;
        private readonly IRepository<Annotation> _annotationRepository;

        public SearchService(
            IRepository<Scan> scanRepository,
            IRepository<Annotation> annotationRepository)
        {
            _scanRepository = scanRepository;
            _annotationRepository = annotationRepository;
        }

        public async Task<IEnumerable<Scan>> Search(SearchQuery query)
        {
            IQueryable<Scan> scans = _scanRepository.GetQueryable(includeProperties: "Recording,Recording.Patient");

            IEnumerable<Scan> scanList = scans.Where(s =>
                s.Recording.Patient.Acronym.ToLower().Contains(query.Name.ToLower())
            && s.Recording.RecordingNumber.ToLower().Contains(query.Rec.ToLower())
            && s.ScanNumber.ToLower().Contains(query.Scan.ToLower())
            && s.Version.ToLower().Contains(query.Ver.ToLower())
            && s.PatientInfo.ToLower().Contains(query.PInfo.ToLower())
            && s.RecordInfo.ToLower().Contains(query.RecInfo.ToLower())
            && s.Labels.ToLower().Contains(query.Label.ToLower())
            && s.TransducerTypes.ToLower().Contains(query.Type.ToLower())
            && s.PhysicalDimensions.ToLower().Contains(query.Dim.ToLower()))
            .ToList();

            List<string> test = new List<string>();

            // handle time and date search
            List<ScanTimeCollection> timeCollection = scanList.Select(s => new ScanTimeCollection()
            {
                ID = s.ID,
                Date = s.StartDate.ToString("dd.MM.yyyy"),
                Time = s.StartTime.ToString("HH:mm:ss")
            }).ToList();

            scanList = SearchTime(query.Time.ToLower(), scanList, timeCollection);
            scanList = SearchDate(query.Date.ToLower(), scanList, timeCollection);

            // handle annotation search
            scanList = SearchAnnotations(query.Annot.ToLower(), scanList);

            return scanList;
        }

        private class ScanTimeCollection
        {
            public int ID { get; set; }
            public string Date { get; set; }
            public string Time { get; set; }
        }

        private IEnumerable<Scan> SearchTime(string time, IEnumerable<Scan> scanList, List<ScanTimeCollection> timeCollection)
        {
            if (!String.IsNullOrEmpty(time))
            {
                // get every ScanTimeCollection where Time contains Query.Time
                // select only the ID property of the ScanTimeCollection objects and store in scanIDs
                // get every scan where the ID appears inside the scanID list
                List<int> scanIDs = new List<int>();
                scanIDs.AddRange(timeCollection
                    .Where(t => t.Time.Contains(time))
                    .Select(t => t.ID));
                return scanList.Where(s => scanIDs.Contains(s.ID));
            }

            return scanList;
        }

        private IEnumerable<Scan> SearchDate(string date, IEnumerable<Scan> scanList, List<ScanTimeCollection> timeCollection)
        {
            if (!String.IsNullOrEmpty(date))
            {
                // get every ScanTimeCollection where Date contains Query.Date
                // select only the ID property of the ScanTimeCollection objects and store in scanIDs
                // get every scan where the ID appears inside the scanID list
                List<int> scanIDs = new List<int>();
                scanIDs.AddRange(timeCollection
                    .Where(t => t.Date.Contains(date))
                    .Select(t => t.ID));
                return scanList.Where(s => scanIDs.Contains(s.ID));
            }

            return scanList;
        }

        private IEnumerable<Scan> SearchAnnotations(string annotation, IEnumerable<Scan> scanList)
        {
            if (!String.IsNullOrEmpty(annotation))
            {
                // get every annotation where description contains Query.Annot
                // select only the ScanID of the Annotation object and store in scanIDs
                // get every scan where the ID appears inside the scanID list
                List<int> scanIDs = new List<int>();
                List<Annotation> annotations = _annotationRepository.GetQueryable().ToList();
                annotations = annotations.Where(a => a.Description.ToLower().Contains(annotation)).ToList();
                scanIDs.AddRange(annotations.Select(a => a.ScanID));
                return scanList.Where(s => scanIDs.Contains(s.ID));
            }

            return scanList;
        }
    }
}
