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
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Collections.ObjectModel;



namespace prakt_project.window
{
    /// <summary>
    /// Логика взаимодействия для changeStudentWindow.xaml
    /// </summary>
    public partial class changeStudentWindow : Window
    {


        private string connectionString = "Data Source=(localdb)\\MSSQLLocalDB; Initial Catalog=schoolDB; Integrated Security=True";

        prakt_project.MainWindow.Student student = new prakt_project.MainWindow.Student();
        
        public changeStudentWindow(prakt_project.MainWindow.Student selectedStudent)
        {
            InitializeComponent();
            student = selectedStudent;
            selectAllClassToComboBox();
            selectActualClassInfo(selectedStudent);


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
        private void selectActualClassInfo(MainWindow.Student selectedStudent)
        {
            фамилияTextBox.Text = selectedStudent.Фамилия.ToString();
            имяTextBox.Text = selectedStudent.Имя.ToString();
            отчествоTextBox.Text = selectedStudent.Отчество.ToString();
            comboBoxClasses.SelectedItem = selectedStudent.Класс.ToString();

           
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int tempId=-1;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = $"Select Класс_ID from tbl_Classes Where Название_класса = N'{comboBoxClasses.SelectedItem.ToString()}'";
                    SqlCommand command = new SqlCommand(query, connection);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read()) 
                        {
                            tempId = Convert.ToInt32(reader["Класс_ID"]);
                        }
                        else
                        {
                            MessageBox.Show("Данные не найдены", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                            this.Hide();
                        }
                    }
                }
                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        string query = $"UPDATE tbl_Student " +
                        $"SET " +
                        $"Фамилия = @Фамилия, " +
                        $"Имя = @Имя, " +
                        $"Отчество = @Отчество, " +
                        $"Класс_ID = @Класс_ID " +
                        $"WHERE Ученик_ID = @Ученик_ID";

                        SqlCommand command = new SqlCommand(query, connection);
                        command.Parameters.AddWithValue("@Фамилия", фамилияTextBox.Text);
                        command.Parameters.AddWithValue("@Имя", имяTextBox.Text);
                        command.Parameters.AddWithValue("@Отчество", отчествоTextBox.Text);
                        command.Parameters.AddWithValue("@Класс_ID", tempId);
                        command.Parameters.AddWithValue("@Ученик_ID", student.Ученик_ID);
                        
                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Запись успешно обновлена", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                            
                        }
                        else
                        {
                            MessageBox.Show("Не удалось обновить запись", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                           
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            ChildWindowClosed?.Invoke(this, EventArgs.Empty);
            this.Hide();



        }

        public event EventHandler ChildWindowClosed;

        private void Button_Click_Close(object sender, RoutedEventArgs e)
        {
            ChildWindowClosed?.Invoke(this, EventArgs.Empty);
            this.Hide();
        }



    }

}
