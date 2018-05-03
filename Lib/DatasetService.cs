﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Geonorge.MassivNedlasting
{
    public class DatasetService
    {
        private static readonly HttpClient HttpClient = new HttpClient();

        public List<Dataset> GetDatasets()
        {
            try
            {
                var getFeedTask = HttpClient.GetStringAsync("https://nedlasting.geonorge.no/geonorge/Tjenestefeed.xml");
                return new AtomFeedParser().ParseDatasets(getFeedTask.Result);
            }
            catch (Exception e)
            {
                MessageBox.Show("Klarer ikke hente fra tjenestefeed");
            }

            return new List<Dataset>();
        }

        public List<DatasetFileViewModel> GetDatasetFiles(Dataset dataset)
        {
            var getFeedTask = HttpClient.GetStringAsync(dataset.Url);
            List<DatasetFile> datasetFiles = new AtomFeedParser().ParseDatasetFiles(getFeedTask.Result, dataset).OrderBy(d => d.Title).ToList();

            return ConvertToViewModel(datasetFiles);
        }

        public DatasetFile GetDatasetFile(DatasetFile originalDatasetFile)
        {
            var getFeedTask = HttpClient.GetStringAsync(originalDatasetFile.DatasetUrl);
            return new AtomFeedParser().ParseDatasetFile(getFeedTask.Result, originalDatasetFile);
        }

        /// <summary>
        /// Writes the information about the selected files to the local download list. 
        /// </summary>
        /// <param name="datasetFilesViewModel"></param>
        public void WriteToDownloadFile(List<DatasetFileViewModel> datasetFilesViewModel)
        {
            List<DatasetFile> datasetFiles = ConvertToModel(datasetFilesViewModel);
            var serializer = new JsonSerializer();
            serializer.Converters.Add(new JavaScriptDateTimeConverter());
            serializer.NullValueHandling = NullValueHandling.Ignore;

            using (var outputFile = new StreamWriter(ApplicationService.GetDownloadFilePath(), false))
            using (JsonWriter writer = new JsonTextWriter(outputFile))
            {
                serializer.Serialize(writer, datasetFiles);
                writer.Close();
            }
        }

        /// <summary>
        /// Writes the information about the selected files to the local download list. 
        /// </summary>
        /// <param name="datasetFilesViewModel"></param>
        public void WriteToDownloadFile(List<DatasetFile> datasetFiles)
        {
            var serializer = new JsonSerializer();
            serializer.Converters.Add(new JavaScriptDateTimeConverter());
            serializer.NullValueHandling = NullValueHandling.Ignore;

            using (var outputFile = new StreamWriter(ApplicationService.GetDownloadFilePath(), false))
            using (JsonWriter writer = new JsonTextWriter(outputFile))
            {
                serializer.Serialize(writer, datasetFiles);
                writer.Close();
            }
        }

        private List<DatasetFile> ConvertToModel(List<DatasetFileViewModel> datasetFilesViewModel)
        {
            var datasetFiles = new List<DatasetFile>();
            foreach (var datasetFileViewModel in datasetFilesViewModel)
            {
                var datasetFile = new DatasetFile(datasetFileViewModel);
                datasetFiles.Add(datasetFile);
            }
            return datasetFiles;
        }

        /// <summary>
        /// Returns a list of dataset files to download. 
        /// </summary>
        /// <returns></returns>
        public List<DatasetFile> GetSelectedFiles()
        {
            try
            {
                using (var r = new StreamReader(ApplicationService.GetDownloadFilePath()))
                {
                    var json = r.ReadToEnd();
                    var selecedFiles = JsonConvert.DeserializeObject<List<DatasetFile>>(json);
                    r.Close();
                    return selecedFiles;
                }
            }
            catch (Exception)
            {
                // TODO error handling
                return new List<DatasetFile>();
            }
        }

        public List<DatasetFileViewModel> GetSelectedFilesAsViewModel()
        {
            List<DatasetFile> selectedFiles = GetSelectedFiles();
            return ConvertToViewModel(selectedFiles, true);
        }

        private List<DatasetFileViewModel> ConvertToViewModel(List<DatasetFile> datasetFiles, bool selectedForDownload = false)
        {
            var selectedFilesViewModel = new List<DatasetFileViewModel>();
            foreach (var selectedFile in datasetFiles)
            {
                DatasetFileViewModel selectedFileViewModel = new DatasetFileViewModel(selectedFile, selectedForDownload);
                selectedFilesViewModel.Add(selectedFileViewModel);
            }
            return selectedFilesViewModel;
        }
    }
}