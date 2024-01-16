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
                string[] first = new string[] { "Good" };
                string[] last = new string[] { "Good1" };
                T2CardGen nn = new T2CardGen();
                nn.genDock(@"C:\Users\valer\source\repos\prakt_project\prakt_project\pattern_Docx\СПРАВКА.docx", textBoxPath.Text + @"\" + $"{student.Фамилия}_Справка" +".docx", first, last) ;
            }
            if(comboBoxDocx.SelectedIndex==1)
            {

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
