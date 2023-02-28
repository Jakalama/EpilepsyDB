using EpilepsieDB.Services;
using EpilepsieDB.Web.View.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace EpilepsieDB.Tests.Web.View.Parser
{
    public class SearchInputParserTest : AbstractTest
    {
        public SearchInputParserTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Parse()
        {
            // set
            string search = "";

            SearchInputParser parser = new SearchInputParser();

            // act
            SearchQuery query = parser.Parse(search);

            // assert
            Assert.NotNull(query);
        }

        [Fact]
        public void Parse_ReturnsNotNull_IfSearchIsNull()
        {
            // set
            SearchInputParser parser = new SearchInputParser();

            // act
            SearchQuery query = parser.Parse(null);

            // assert
            Assert.NotNull(query);
        }

        [Fact]
        public void Parse_ContainsName()
        {
            // set
            string name = "olaf";
            string search = "name:" + name;

            SearchInputParser parser = new SearchInputParser();

            // act
            SearchQuery query = parser.Parse(search);

            // assert
            Assert.Equal(name, query.Name);
        }

        [Fact]
        public void Parse_ContainsRec()
        {
            // set
            string rec = "olaf";
            string search = "rec:" + rec;

            SearchInputParser parser = new SearchInputParser();

            // act
            SearchQuery query = parser.Parse(search);

            // assert
            Assert.Equal(rec, query.Rec);
        }

        [Fact]
        public void Parse_ContainsScan()
        {
            // set
            string scan = "olaf";
            string search = "scan:" + scan;

            SearchInputParser parser = new SearchInputParser();

            // act
            SearchQuery query = parser.Parse(search);

            // assert
            Assert.Equal(scan, query.Scan);
        }

        [Fact]
        public void Parse_ContainsVer()
        {
            // set
            string ver = "olaf";
            string search = "ver:" + ver;

            SearchInputParser parser = new SearchInputParser();

            // act
            SearchQuery query = parser.Parse(search);

            // assert
            Assert.Equal(ver, query.Ver);
        }

        [Fact]
        public void Parse_ContainsPInfo()
        {
            // set
            string pinfo = "olaf";
            string search = "pinfo:" + pinfo;

            SearchInputParser parser = new SearchInputParser();

            // act
            SearchQuery query = parser.Parse(search);

            // assert
            Assert.Equal(pinfo, query.PInfo);
        }

        [Fact]
        public void Parse_ContainsRecInfo()
        {
            // set
            string recInfo = "olaf";
            string search = "recinfo:" + recInfo;

            SearchInputParser parser = new SearchInputParser();

            // act
            SearchQuery query = parser.Parse(search);

            // assert
            Assert.Equal(recInfo, query.RecInfo);
        }

        [Fact]
        public void Parse_ContainsDate()
        {
            // set
            string date = "olaf";
            string search = "date:" + date;

            SearchInputParser parser = new SearchInputParser();

            // act
            SearchQuery query = parser.Parse(search);

            // assert
            Assert.Equal(date, query.Date);
        }

        [Fact]
        public void Parse_ContainsTime()
        {
            // set
            string time = "olaf";
            string search = "time:" + time;

            SearchInputParser parser = new SearchInputParser();

            // act
            SearchQuery query = parser.Parse(search);

            // assert
            Assert.Equal(time, query.Time);
        }

        [Fact]
        public void Parse_ContainsLabel()
        {
            // set
            string label = "olaf";
            string search = "label:" + label;

            SearchInputParser parser = new SearchInputParser();

            // act
            SearchQuery query = parser.Parse(search);

            // assert
            Assert.Equal(label, query.Label);
        }

        [Fact]
        public void Parse_ContainsType()
        {
            // set
            string type = "olaf";
            string search = "type:" + type;

            SearchInputParser parser = new SearchInputParser();

            // act
            SearchQuery query = parser.Parse(search);

            // assert
            Assert.Equal(type, query.Type);
        }

        [Fact]
        public void Parse_ContainsDim()
        {
            // set
            string dim = "olaf";
            string search = "dim:" + dim;

            SearchInputParser parser = new SearchInputParser();

            // act
            SearchQuery query = parser.Parse(search);

            // assert
            Assert.Equal(dim, query.Dim);
        }

        [Fact]
        public void Parse_ContainsAnnot()
        {
            // set
            string annot = "olaf";
            string search = "annot:" + annot;

            SearchInputParser parser = new SearchInputParser();

            // act
            SearchQuery query = parser.Parse(search);

            // assert
            Assert.Equal(annot, query.Annot);
        }

        [Fact]
        public void Parse_ContainsAll()
        {
            // set
            string name = "olaf";
            string rec = "olaf";
            string scan = "olaf";
            string ver = "olaf";
            string pInfo = "olaf";
            string recInfo = "olaf";
            string date = "olaf";
            string time = "olaf";
            string label = "olaf";
            string type = "olaf";
            string dim = "olaf";
            string annot = "olaf";

            string search = $"name:{name} rec:{rec} scan:{scan} ver:{ver} pinfo:{pInfo} recinfo:{recInfo} date:{date} time:{time} label:{label} type:{type} dim:{dim} annot:{annot}";

            SearchInputParser parser = new SearchInputParser();

            // act
            SearchQuery query = parser.Parse(search);

            // assert
            Assert.Equal(name, query.Name);
            Assert.Equal(rec, query.Rec);
            Assert.Equal(scan, query.Scan);
            Assert.Equal(ver, query.Ver);
            Assert.Equal(pInfo, query.PInfo);
            Assert.Equal(recInfo, query.RecInfo);
            Assert.Equal(date, query.Date);
            Assert.Equal(time, query.Time);
            Assert.Equal(label, query.Label);
            Assert.Equal(type, query.Type);
            Assert.Equal(dim, query.Dim);
            Assert.Equal(annot, query.Annot);

        }
    }
}
