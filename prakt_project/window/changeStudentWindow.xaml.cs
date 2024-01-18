using System;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Forms;
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
            фамилияTextBox.TextChanged += TextBox_TextChanged;
            имяTextBox.TextChanged += TextBox_TextChanged;
            отчествоTextBox.TextChanged += TextBox_TextChanged;

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
                System.Windows.Forms.MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}", "Ошибка", (MessageBoxButtons)MessageBoxButton.OK, (MessageBoxIcon)MessageBoxImage.Error);
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
                            System.Windows.Forms.MessageBox.Show("Данные не найдены", "Предупреждение", (MessageBoxButtons)MessageBoxButton.OK, (MessageBoxIcon)MessageBoxImage.Warning);
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
                            System.Windows.Forms.MessageBox.Show("Запись успешно обновлена", "Успех", (MessageBoxButtons)MessageBoxButton.OK, (MessageBoxIcon)MessageBoxImage.Information);
                            
                        }
                        else
                        {
                            System.Windows.Forms.MessageBox.Show("Не удалось обновить запись", "Ошибка", (MessageBoxButtons)MessageBoxButton.OK, (MessageBoxIcon)MessageBoxImage.Error);
                           
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}", "Ошибка", (MessageBoxButtons)MessageBoxButton.OK, (MessageBoxIcon)MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}", "Ошибка", (MessageBoxButtons)MessageBoxButton.OK, (MessageBoxIcon)MessageBoxImage.Error);
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

        private void фамилияTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            bool isValidInput = фамилияTextBox.Text.All(char.IsLetter) &&
                                имяTextBox.Text.All(char.IsLetter) &&
                                отчествоTextBox.Text.All(char.IsLetter);
            buttonOK.IsEnabled = isValidInput &&
                                 !string.IsNullOrWhiteSpace(фамилияTextBox.Text) &&
                                 !string.IsNullOrWhiteSpace(имяTextBox.Text) &&
                                 !string.IsNullOrWhiteSpace(отчествоTextBox.Text);
            фамилияTextBox.Background = фамилияTextBox.Text.All(char.IsLetter) ? System.Windows.Media.Brushes.White : System.Windows.Media.Brushes.LightCoral;
            имяTextBox.Background = имяTextBox.Text.All(char.IsLetter) ? System.Windows.Media.Brushes.White : System.Windows.Media.Brushes.LightCoral;
            отчествоTextBox.Background = отчествоTextBox.Text.All(char.IsLetter) ? System.Windows.Media.Brushes.White : System.Windows.Media.Brushes.LightCoral;
        }
    }

}
