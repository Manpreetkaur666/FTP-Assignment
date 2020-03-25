using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CSV.Models;
using CSV.Models.Utilities;
using System.Xml.Serialization;


namespace CSV
{
    class Program
    {
        static void Main(string[] args)
        {
            Student myrecord = new Student { StudentId = "200429017", FirstName = "Manpreet", LastName = "Kaur" };

          


            List<string> directories = FTP.GetDirectory(Constants.FTP.BaseUrl);
            List<Student> students = new List<Student>();

            foreach (var directory in directories)
            {
                Student student = new Student() { AbsoluteUrl = Constants.FTP.BaseUrl };
                student.FromDirectory(directory);

                //Console.WriteLine(student);
                string infoFilePath = student.FullPathUrl + "/" + Constants.Locations.InfoFile;

                bool fileExists = FTP.FileExists(infoFilePath);
                if (fileExists == true)
                {
                   
                    string csvPath = $@"/Users/manpreetkaur/Desktop/data/{directory}.csv";

                    // FTP.DownloadFile(infoFilePath, csvPath);
                    byte[] bytes = FTP.DownloadFileBytes(infoFilePath);
                    string csvData = Encoding.Default.GetString(bytes);

                    string[] csvlines = csvData.Split("\r\n", StringSplitOptions.RemoveEmptyEntries);

                    if (csvlines.Length != 2)
                    {
                        Console.WriteLine("Error in CSV format");
                    }
                    else
                    {
                        student.FromCSV(csvlines[1]);
                        //Console.WriteLine("  \t Age of Student is: {0} ", student.age);
                    }

                    Console.WriteLine("Found info file:");
                }
                else
                {
                    Console.WriteLine("Could not find info file:");
                }

                Console.WriteLine("\t" + infoFilePath);

                string imageFilePath = student.FullPathUrl + "/" + Constants.Locations.ImageFile;

                bool imageFileExists = FTP.FileExists(imageFilePath);

                if (imageFileExists == true)
                {

                    Console.WriteLine("Found image file:");
                }
                else
                {
                    Console.WriteLine("Could not find image file:");
                }

                Console.WriteLine("\t" + imageFilePath);

                students.Add(student);
                Console.WriteLine(directory);

                Console.WriteLine(" \t Count of student is: {0}", students.Count);
                Console.WriteLine("  \t Age of Student is: {0} ", student.age);

            }

            Student me = students.SingleOrDefault(x => x.StudentId == myrecord.StudentId);
            Student meUsingFind = students.Find(x => x.StudentId == myrecord.StudentId);

            var avgage = students.Average(x => x.age);
            var minage = students.Min(x => x.age);
            var maxage = students.Max(x => x.age);


            Console.WriteLine("  \n\t Name Searched With Query: {0} ", meUsingFind);
            Console.WriteLine("  \t Average of Student age is: {0} ", avgage);
            Console.WriteLine("  \t Minimum of Student age is: {0} ", minage);
            Console.WriteLine("  \t Maximum of Student age is: {0} ", maxage);

            
            string studentsCSVPath = $"{Constants.Locations.DataFolder}//students.csv";
            //Establish a file stream to collect data from the response
            using (StreamWriter fs = new StreamWriter(studentsCSVPath))
            {
                foreach (var student in students)
                {
                    fs.WriteLine(student.ToCSV());
                }
            }

            string studentsjsonPath = $"{Constants.Locations.DataFolder}//students.json";
            //Establish a file stream to collect data from the response
            using (StreamWriter fs = new StreamWriter(studentsjsonPath))
            {
                foreach (var student in students)
                {
                    string Student = Newtonsoft.Json.JsonConvert.SerializeObject(student);
                    fs.WriteLine(Student.ToString());
                    //Console.WriteLine(jStudent);
                }
            }

            //string studentsxmlPath = $"{Constants.Locations.DataFolder}//students.xml";
            //XmlSerializer serializer = new XmlSerializer(typeof(Student));
            //using (StreamWriter fs = new StreamWriter(studentsxmlPath))
            //{
            //    serializer.Serialize(fs, students);
            //}


                string studentsxmlPath = $"{Constants.Locations.DataFolder}//students.xml";
                //Establish a file stream to collect data from the response
                using (StreamWriter fs = new StreamWriter(studentsxmlPath))
                {
                //foreach (var student in students)
                //{
                //    // XmlSerializer xs = new XmlSerializer(student.GetType());
                //    XmlSerializer xs = new XmlSerializer(typeof(Student));

                //    //xs.Serialize(fs, student);
                //    fs.WriteLine(student);

                XmlSerializer x = new XmlSerializer(students.GetType());
                x.Serialize(fs, students);
                Console.WriteLine();

                //XmlSerializer x = new XmlSerializer(myrecord.GetType());
                //x.Serialize(Console.Out, myrecord);
                //Console.ReadKey();


                //Test myTest = new Test() { value1 = "Value 1", value2 = "Value 2" };
                //XmlSerializer x = new XmlSerializer(myTest.GetType());
                //x.Serialize(Console.Out, myTest);
                //Console.ReadKey();


            }

            
            //}

            return;

            //string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);

            //string exePath = Environment.CurrentDirectory;
            //string dataFolder = $"{exePath}\\..\\..\\..\\Content\\Data";
            //string imagesFolder = $"{exePath}\\..\\..\\..\\Content\\Images";

            //string filePath = $@"{Constants.Locations.DataFolder}\{Constants.Locations.InfoFile}";
            //string fileContents;

            //using (StreamReader stream = new StreamReader(filePath))
            //{
            //    fileContents = stream.ReadToEnd();
            //}

            //List<string> entries = new List<string>();

            //entries = fileContents.Split("\r\n", StringSplitOptions.RemoveEmptyEntries).ToList();

            //Student student = new Student();
            //student.FromCSV(entries[1]);



            //string[] data = entries[1].Split(",", StringSplitOptions.None);

            //Student student = new Student();
            //student.StudentId = data[0];
            //student.FirstName = data[1];
            //student.LastName = data[2];
            //student.DateOfBirth = data[3];
            //student.ImageData = data[4];

            //Console.WriteLine(student.ToCSV());
            //Console.WriteLine(student.ToString());



            //string imagefilePath = $"{Constants.Locations.ImagesFolder}\\{Constants.Locations.ImageFile}";
            //Image image = Image.FromFile(imagefilePath);
            //string base64Image = Imaging.ImageToBase64(image, ImageFormat.Jpeg);
            //student.ImageData = base64Image;

            //string newfilePath = $"{Constants.Locations.DesktopPath}\\{student.ToString()}.jpg";
            //FileInfo newfileinfo = new FileInfo(newfilePath);
            //Image studentImage = Imaging.Base64ToImage(student.ImageData);
            //studentImage.Save(newfileinfo.FullName, ImageFormat.Jpeg);
        }

    }
}
