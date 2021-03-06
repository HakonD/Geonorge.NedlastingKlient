﻿using Geonorge.MassivNedlasting;
using System.Collections.Generic;

namespace Geonorge.Nedlaster
{
    public class DatasetFileLog
    {
        private DatasetFile localDataset;

        public DatasetFileLog(DatasetFile localDataset)
        {
            DatasetId = localDataset.DatasetId;
            Name = localDataset.Title;
            Projection = localDataset.Projection;
        }

        public DatasetFileLog()
        {
            DatasetId = localDataset.DatasetId;
        }

        public string DatasetId { get; set; }
        public string Name { get; set; }
        public string Projection { get; set; }
        public int NumberOfFilesUpdated { get; set; }
        public int TotalNumberOfFiles { get; set; }
        public string HumanReadableSize { get; set; }
        public string Message { get; set; }

    }
}