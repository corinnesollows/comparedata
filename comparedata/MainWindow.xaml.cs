using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;

namespace comparedata
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private string appPath = AppDomain.CurrentDomain.BaseDirectory;//gets the directory the .exe is in
        private string filePath = AppDomain.CurrentDomain.BaseDirectory + @"Files\";//gets the directory the files go in/are in
        private string filePath2 = AppDomain.CurrentDomain.BaseDirectory + @"Files2\";//gets the directory the files go in/are in

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnCompareFiles_Click(object sender, RoutedEventArgs e)
        {
            //List<string> firstTestSet = new List<string>();
            List<string> file1Lines = new List<string>();
            //List<string> secondTestSet = new List<string>();
            List<string> file2Lines = new List<string>();
            string[] fileEntries = Directory.GetFiles(filePath); //gets full all file names from dir
      
        string[] fileEntries2 = Directory.GetFiles(filePath2); //gets full all file names from dir

            //FileInfo fileInfo1 = new FileInfo(fileEntries[0]);
            //FileInfo fileInfo2 = new FileInfo(fileEntries2[0]);
            //if (fileInfo1.Length != fileInfo2.Length)
            //{
            //    MessageBox.Show("file lengths are not the same. Check spacing or content", "Completed", MessageBoxButton.OK);
            //}
            //CombineMultipleFilesIntoSingleFile(filePath, filePath + "Everything.csv");
            //CombineMultipleFilesIntoSingleFile(filePath2, filePath2 + "Everything2.csv");

            foreach (string file in fileEntries)
            {
                bool same = true;
                FileInfo fileInfo1 = new FileInfo(file);
                file1Lines = FileRead(file);
                string file2Name = "";
                foreach (string file2 in fileEntries2)
                {
                    file2Lines = FileRead(file2);
                    if (file2Lines.ElementAt(0) == file1Lines.ElementAt(0))
                    {
                        FileInfo fileInfo2 = new FileInfo(file2);
                        file2Name = fileInfo2.Name;
                        if (fileInfo1.Length != fileInfo2.Length)
                        {
                            same = false;
                            MessageBox.Show("file lengths are not the same. Check spacing or content " + fileInfo1.Name + " And " + fileInfo2.Name, "Completed", MessageBoxButton.OK);
                        }
                        break;
                    }
                }

                for(int i = 0; i < file1Lines.Count;i++)
                {
                    if (file1Lines.ElementAt(i).Equals(file2Lines.ElementAt(i)))
                    {
                    }
                    else
                    {
                        same = false;
                        int linenumber = i + 1;
                        MessageBox.Show("There is an inconsitant line! Line number " + System.Environment.NewLine + linenumber + "!" + System.Environment.NewLine + fileInfo1.Name + " " + file1Lines.ElementAt(i) + 
                            System.Environment.NewLine + file2Name + " " + file2Lines.ElementAt(i), "Completed", MessageBoxButton.OK);
                        if (file1Lines.ElementAt(linenumber).Equals(file2Lines.ElementAt(linenumber)))
                        {
                        }
                        else
                        {
                            MessageBox.Show("File is probably thrown off of alignment", "Completed", MessageBoxButton.OK);
                            break;
                        }
                    }
                }
                if(same == true)
                    MessageBox.Show(fileInfo1.Name  + " and " + file2Name + " Exactly the same!", "Completed", MessageBoxButton.OK);
            }
        }

        private static void CombineMultipleFilesIntoSingleFile(string inputDirectoryPath, string outputFilePath)
        {
            string[] inputFilePaths = Directory.GetFiles(inputDirectoryPath);
            Console.WriteLine("Number of files: {0}.", inputFilePaths.Length);
            using (var outputStream = File.Create(outputFilePath))
            {
                foreach (var inputFilePath in inputFilePaths)
                {
                    using (var inputStream = File.OpenRead(inputFilePath))
                    {
                        inputStream.CopyTo(outputStream);
                    }
                    Console.WriteLine("The file {0} has been processed.", inputFilePath);
                }
            }
        }
        public List<string> FileRead(string fP)
        {
            List<string> fileLines = new List<string>();
            try//read file 
            {
                foreach (string line in File.ReadAllLines(fP))
                {
                    fileLines.Add(line.TrimEnd()); //split parameters into list values
                }
                return fileLines;
            }
            catch
            {
                MessageBox.Show("File error", "File error", MessageBoxButton.OK);
                return fileLines;
            }
        }

        private void btncreateUSDPFiles_Click(object sender, RoutedEventArgs e)
        {
            List<string> fileContents = FileRead(filePath + "\\List2DO.txt");
            for (int i = 0; i < 10; i++)
            {
                string fileName = "USDPList"+i+".txt";
                using (StreamWriter usdpfile = File.AppendText(filePath + fileName)) //creates file of USDPs
                {
                    for (int index = i; index < fileContents.Count; index += 10)
                    {
                        usdpfile.WriteLine(fileContents.ElementAt(index));
                    }
                }
            }
        }

        private void btnremoveDuplicates_Click(object sender, RoutedEventArgs e)
        {
            string[] fileEntries = Directory.GetFiles(filePath); //gets full all file names from dir
            foreach (string file in fileEntries)
            {
                List<string> fileLines = FileRead(file).Distinct().ToList();
                string fileName = "USDPListNEW.txt";
                using (StreamWriter usdpfile = File.AppendText(filePath + fileName)) //creates file of USDPs
                {
                    for (int index = 0; index < fileLines.Count; index++)
                    {
                        usdpfile.WriteLine(fileLines.ElementAt(index));
                    }
                }
                List<string> duplicateKeys = FileRead(file).GroupBy(x => x).Where(group => group.Count() > 1).Select(group => group.Key).ToList();
                string fileName1 = "USDPDuplicates.txt";
                using (StreamWriter usdpfileduplicates = File.AppendText(filePath + fileName1)) //creates file of USDPs
                {
                    for (int index = 0; index < duplicateKeys.Count; index++)
                    {
                        usdpfileduplicates.WriteLine(duplicateKeys.ElementAt(index));
                    }
                }
            }
        }
    }
}
