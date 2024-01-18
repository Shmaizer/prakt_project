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
namespace prakt_project.window
{
    /// <summary>
    /// Логика взаимодействия для createStudent.xaml
    /// </summary>
    public partial class createStudent : Window
    {
        private string connectionString = "Data Source=(localdb)\\MSSQLLocalDB; Initial Catalog=schoolDB; Integrated Security=True";

        public createStudent()
        {
            InitializeComponent();
            selectAllClassesToComboBox();
            фамилияTextBox.TextChanged += TextBox_TextChanged;
            имяTextBox.TextChanged += TextBox_TextChanged;
            отчествоTextBox.TextChanged += TextBox_TextChanged;
        }
        public event EventHandler ChildWindowClosed;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int tempId = -1;
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
                            ChildWindowClosed?.Invoke(this, EventArgs.Empty);
                            this.Hide();
                        }
                    }
                }
                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        string query = $"INSERT INTO tbl_Student (Фамилия, Имя, Отчество, Класс_ID, ДатаРождения)" +
                                        $"VALUES(@Фамилия, @Имя, @Отчество, @Класс_ID, @ДатаРождения);";

                        SqlCommand command = new SqlCommand(query, connection);
                        command.Parameters.AddWithValue("@Фамилия", фамилияTextBox.Text);
                        command.Parameters.AddWithValue("@Имя", имяTextBox.Text);
                        command.Parameters.AddWithValue("@Отчество", отчествоTextBox.Text);
                        command.Parameters.AddWithValue("@Класс_ID", tempId);
                        command.Parameters.AddWithValue("@ДатаРождения", dataSelect.SelectedDate);

                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Запись успешно добавлена", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                            ChildWindowClosed?.Invoke(this, EventArgs.Empty);
                            this.Hide();
                        }
                        else
                        {
                            MessageBox.Show("Не удалось добавить запись", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

                        }
                    }


                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при загрузке данных [Update_try_2]: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных[Update_try_1]: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void selectAllClassesToComboBox()
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
                MessageBox.Show($"Ошибка при загрузке данных[selectAllClasses_try_2]: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void comboBoxClasses_SelectionChanged(object sender, SelectionChangedEventArgs e)
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

