using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Animation;
using System.Data.SqlClient;
using System.Data;
using System.Collections.ObjectModel;
using System.Globalization;
namespace prakt_project
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    /// 

    public partial class MainWindow : Window
    {
        private string connectionString = "Data Source=(localdb)\\MSSQLLocalDB; Initial Catalog=schoolDB; Integrated Security=True";
        public class Student
        {
            public int Ученик_ID { get; set; }
            public string ФИО { get; set; }
            public string Фамилия { get; set; }
            public string Имя { get; set; }
            public string Отчество { get; set; }
            public string Класс { get; set; }
            public int Класс_ID { get; set; }
            public string датаСправки { get; set; }
            public string ДатаРождения { get; set; }
        }
        private ObservableCollection<Student> students;
        public MainWindow()
        {
            InitializeComponent();
            selectAllClassToComboBox();
            selectClassesToChangeComboBox();
            
        }

        private void ChildWindow_DataUpdated(object sender, EventArgs e)
        {
            
            selectClassesToChangeComboBox();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectClassesToChangeComboBox();
        }
        private void selectAllClassToComboBox()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT Название_класса FROM tbl_Classes";
                    SqlCommand command = new SqlCommand(query, connection);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            comboBoxClasses.Items.Add(reader["Название_класса"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void selectClassesToChangeComboBox()
        {
            students = new ObservableCollection<Student>();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string selectedClass = comboBoxClasses.SelectedItem.ToString();
                    string query = $"SELECT * FROM УченикИнформация WHERE Класс = N'{selectedClass}'";
                    SqlCommand command = new SqlCommand(query, connection);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            students.Add(new Student
                            {
                                ФИО = reader["Фамилия"].ToString()+" "+ reader["Имя"].ToString()+ " "+ reader["Отчество"].ToString(),
                                Фамилия = reader["Фамилия"].ToString(),
                                Имя = reader["Имя"].ToString(),
                                Отчество = reader["Отчество"].ToString(),
                                Класс = reader["Класс"].ToString(),
                                Класс_ID = Convert.ToInt32(reader["Класс_ID"]),
                                Ученик_ID = Convert.ToInt32(reader["Ученик_ID"]),
                                ДатаРождения = reader["ДатаРождения"].ToString(),
                                датаСправки = (reader["ДатаСправки"] != DBNull.Value) ?
                      (DateTime.Parse(reader["ДатаСправки"].ToString()).Day).ToString() + "." +
                      (DateTime.Parse(reader["ДатаСправки"].ToString()).Month).ToString() + "." +
                      (DateTime.Parse(reader["ДатаСправки"].ToString()).Year).ToString() :
                      "отсутствует"
                            });
                        }
                    }
                }
                dataGridMain.ItemsSource = students;


                


            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных [selectStudent_catch_1]: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }

        private void ChildWindowClosedHandler(object sender, EventArgs e)
        {
            selectClassesToChangeComboBox();
            colorCell();
        }
        private void editStudentInfoButton(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button != null && button.DataContext is Student)
            {
                Student selectedStudent = button.DataContext as Student;
                //MessageBox.Show(selectedStudent.ФИО);
                window.changeStudentWindow childWindow = new window.changeStudentWindow(selectedStudent);
                childWindow.ChildWindowClosed += ChildWindowClosedHandler;
                childWindow.ShowDialog();
                //new window.changeStudentWindow(selectedStudent).ShowDialog();
            }

        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            window.createStudent childWindow = new window.createStudent();
            childWindow.ChildWindowClosed += ChildWindowClosedHandler;
            childWindow.ShowDialog();
        }
        private void printDockToStudentButton(object sender, RoutedEventArgs e)
        {
            //string[] mass = new string[] { "Good" };
            //string[] mass2 = new string[] { "Good1" };
            //T2CardGen nn = new T2CardGen();
            //nn.genDock("E:\\еуые\\1.docx", "E:\\еуые\\1\\1.docx", mass, mass2);
            Button button = sender as Button;
            if (button != null && button.DataContext is Student)
            {
                Student selectedStudent = button.DataContext as Student;
                window.docxGen childWindow = new window.docxGen(selectedStudent);
                childWindow.ChildWindowClosed += ChildWindowClosedHandler;
                childWindow.ShowDialog();
            }




        }
        private void updateDocxSpravka_Window(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button != null && button.DataContext is Student)
            {
                Student selectedStudent = button.DataContext as Student;
                window.updateWindowDocx childWindow = new window.updateWindowDocx(selectedStudent);
                childWindow.ChildWindowClosed += ChildWindowClosedHandler;
                childWindow.ShowDialog();
            }
        }

        private void dataGridMain_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }


        //Краски
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            colorCell();
        }
        private void colorCell()
        {
            foreach (var item in dataGridMain.Items)
            {
                var ученик1 = (Student)item;
                int columnIndex = dataGridMain.Columns.IndexOf(dataGridMain.Columns.First(c => c.Header.ToString() == "Справка до"));

                if (columnIndex >= 0 && ученик1.датаСправки != "отсутствует")
                {
                    var row = (DataGridRow)dataGridMain.ItemContainerGenerator.ContainerFromItem(item);

                    if (row == null)
                    {
                        dataGridMain.UpdateLayout();
                        dataGridMain.ScrollIntoView(item);
                        row = (DataGridRow)dataGridMain.ItemContainerGenerator.ContainerFromItem(item);
                    }

                    if (row != null)
                    {
                        var cellContent = dataGridMain.Columns[columnIndex].GetCellContent(row);
                        if (cellContent != null)
                        {
                            var ячейка = (TextBlock)cellContent;
                            var ученик = (Student)item;
                            if (ученик != null && DateTime.Parse(ученик.датаСправки) < DateTime.Now)
                            {
                                ячейка.Foreground = new SolidColorBrush(Colors.Red);
                            }
                            else
                            {
                                ячейка.Foreground = new SolidColorBrush(Colors.Green);
                            }
                        }
                    }
                }
            }
        }



        

    }
}
