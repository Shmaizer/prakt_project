using System;
using System.Windows;
using System.Windows.Forms;
using System.Globalization;
using System.IO;
namespace prakt_project.window
{
    /// <summary>
    /// Логика взаимодействия для docxGen.xaml
    /// </summary>
    public partial class docxGen : Window
    {
        public event EventHandler ChildWindowClosed;
        prakt_project.MainWindow.Student student = new prakt_project.MainWindow.Student();


        public docxGen(prakt_project.MainWindow.Student selectedStudent)
        {
            InitializeComponent();
            student = selectedStudent;
            textBoxFIO.Text = student.ФИО;



        }

        //ChildWindowClosed?.Invoke(this, EventArgs.Empty);
        //Выбрать место
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            using (var folderBrowserDialog = new FolderBrowserDialog())
            {
                DialogResult result = folderBrowserDialog.ShowDialog();

                if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(folderBrowserDialog.SelectedPath))
                {
                    string selectedFolderPath = folderBrowserDialog.SelectedPath;
                    textBoxPath.Text = selectedFolderPath;
                }
            }
        }
        //Отмена
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Close();
        }
        //СОздать
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if(comboBoxDocx.SelectedIndex==0)
            {
                //string путьКДокументу = ПолучитьПутьКДокументу("pattern_Docx", "Справка.docx");
                //System.Windows.MessageBox.Show(путьКДокументу);
                string[] first = new string[] { "fio","day","mouth","age","date" };
                string[] last = new string[] { student.ФИО,DateTime.Now.Day.ToString(), ПолучитьНазваниеМесяца(DateTime.Now.Month), DateTime.Now.Year.ToString(), DateTime.Now.Day.ToString()+"."+DateTime.Now.Month.ToString() + "." + DateTime.Now.Year.ToString(), };
                T2CardGen nn = new T2CardGen();
                nn.genDock(@"C:\Users\valer\source\repos\prakt_project\prakt_project\pattern_Docx\СПРАВКА.docx", textBoxPath.Text + @"\" + $"{student.Фамилия}_Справка" +".docx", first, last) ;
                ChildWindowClosed?.Invoke(this, EventArgs.Empty);
                Close();
            }
            if(comboBoxDocx.SelectedIndex==1)
            {
                string[] first = new string[] { "fio", "class", "date" };
                string[] last = new string[] { student.ФИО, student.Класс, DateTime.Parse(student.ДатаРождения).Day+"."+ DateTime.Parse(student.ДатаРождения).Month + "." + DateTime.Parse(student.ДатаРождения).Year };
                T2CardGen nn = new T2CardGen();
                nn.genDock(@"C:\Users\valer\source\repos\prakt_project\prakt_project\pattern_Docx\СПРАВКА_подтверждение.docx", textBoxPath.Text + @"\" + $"{student.Фамилия}_СПРАВКА_подтверждение" + ".docx", first, last);
                ChildWindowClosed?.Invoke(this, EventArgs.Empty);
                Close();
            }
        }

        static string ПолучитьНазваниеМесяца(int номерМесяца)
        {
            if (номерМесяца >= 1 && номерМесяца <= 12)
            {
                CultureInfo культураРусская = new CultureInfo("ru-RU");
                DateTimeFormatInfo информацияОВремени = культураРусская.DateTimeFormat;
                string названиеМесяца = информацияОВремени.GetMonthName(номерМесяца);
                return названиеМесяца;
            }
            else
            {
                return "Некорректный номер месяца";
            }
        }





        static string ПолучитьПутьКДокументу(string папка, string имяФайла)
        {
            string путьКПапкеДокументов = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, папка);
            string полныйПуть = Path.Combine(путьКПапкеДокументов, имяФайла);
            return полныйПуть;
        }
    }
}
