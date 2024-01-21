using System;
using System.Windows;
using System.Windows.Forms;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using NPetrovich;
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
            textBoxFIO.TextChanged += TextBox_TextChanged;


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
                var petrovich = new Petrovich()
                {
                    FirstName = student.Имя.ToString(),
                    LastName = student.Фамилия.ToString(),
                    MiddleName = student.Отчество.ToString(),
                    AutoDetectGender = true
                };
                var inflected = petrovich.InflectTo(Case.Dative);
                string[] first = new string[] { "fio","day","mouth","age","date", "class" };
                string[] last = new string[] {
                    inflected.FirstName+" "+inflected.LastName+" "+inflected.MiddleName,
                    DateTime.Now.Day.ToString(),
                    ПолучитьНазваниеМесяца(DateTime.Now.Month), 
                    
                    DateTime.Now.Year.ToString(), 
                    DateTime.Now.Day.ToString()+"."+DateTime.Now.Month.ToString() + "." + DateTime.Now.Year.ToString(),
                    student.Класс};
                T2CardGen nn = new T2CardGen();
                nn.genDock(@"C:\Users\valer\source\repos\prakt_project\prakt_project\pattern_Docx\СПРАВКА.docx", textBoxPath.Text + @"\" + $"{student.Фамилия}_Справка" +".docx", first, last) ;
                System.Windows.Forms.MessageBox.Show("Справка была успешно создана!");

                ChildWindowClosed?.Invoke(this, EventArgs.Empty);
                Close();
            }
            if(comboBoxDocx.SelectedIndex==1)
            {
                var petrovich = new Petrovich()
                {
                    FirstName = student.Имя.ToString(),
                    LastName = student.Фамилия.ToString(),
                    MiddleName = student.Отчество.ToString(),
                    AutoDetectGender = true
                };
                
                string[] first = new string[] { "fio", "class", "date" };
                string[] last = new string[] {
                    petrovich.InflectFirstNameTo(Case.Genitive)+" "+petrovich.InflectLastNameTo(Case.Genitive)+" "+petrovich.InflectMiddleNameTo(Case.Genitive),
                    student.Класс, 
                    DateTime.Parse(student.ДатаРождения).Day+"."+ DateTime.Parse(student.ДатаРождения).Month + "." + DateTime.Parse(student.ДатаРождения).Year };
                T2CardGen nn = new T2CardGen();
                nn.genDock(@"C:\Users\valer\source\repos\prakt_project\prakt_project\pattern_Docx\СПРАВКА_подтверждение.docx", textBoxPath.Text + @"\" + $"{student.Фамилия}_СПРАВКА_подтверждение" + ".docx", first, last);
                ChildWindowClosed?.Invoke(this, EventArgs.Empty);
                System.Windows.Forms.MessageBox.Show("Справка-подтверждениеа была успешно создана!");
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

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            bool isValidInput = textBoxFIO.Text.All(c => char.IsLetter(c) || c == ' ');
            buttonOK.IsEnabled = isValidInput &&
                                 !string.IsNullOrWhiteSpace(textBoxFIO.Text);
            textBoxFIO.Background = textBoxFIO.Text.All(c => char.IsLetter(c) || c == ' ') ? System.Windows.Media.Brushes.White : System.Windows.Media.Brushes.LightCoral;
        }

    }
}
