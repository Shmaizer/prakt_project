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
using System.Data;
using System.Collections.ObjectModel;
using System.Globalization;
namespace prakt_project.window
{
    /// <summary>
    /// Логика взаимодействия для updateWindowDocx.xaml
    /// </summary>
    public partial class updateWindowDocx : Window
    {
        prakt_project.MainWindow.Student student = new prakt_project.MainWindow.Student();
        private string connectionString = "Data Source=(localdb)\\MSSQLLocalDB; Initial Catalog=schoolDB; Integrated Security=True";
        public event EventHandler ChildWindowClosed;


        public updateWindowDocx(prakt_project.MainWindow.Student selectedStudent)
        {
            InitializeComponent();
            student = selectedStudent;
            dataPickerDocx.SelectedDate = (student.датаСправки != "отсутствует") ? DateTime.Parse(student.датаСправки) : DateTime.Now;
        }
        

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Проверяем наличие справки для ученика
                    string checkQuery = "SELECT Документ_ID FROM tbl_Docx WHERE Ученик_ID = @Ученик_ID";
                    SqlCommand checkCommand = new SqlCommand(checkQuery, connection);
                    checkCommand.Parameters.AddWithValue("@Ученик_ID", student.Ученик_ID);

                    object docId = checkCommand.ExecuteScalar();

                    if (docId == null)
                    {
                        // Если справки нет, создаем новую
                        string insertQuery = "INSERT INTO tbl_Docx (ДатаСправки, Ученик_ID) VALUES (@ДатаСправки, @Ученик_ID)";
                        SqlCommand insertCommand = new SqlCommand(insertQuery, connection);
                        insertCommand.Parameters.AddWithValue("@ДатаСправки", dataPickerDocx.SelectedDate);
                        insertCommand.Parameters.AddWithValue("@Ученик_ID", (int)student.Ученик_ID);

                        int rowsInserted = insertCommand.ExecuteNonQuery();

                        if (rowsInserted > 0)
                        {
                            MessageBox.Show("Справка успешно создана", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                            Close();
                        }
                        else
                        {
                            MessageBox.Show("Не удалось создать справку", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    else
                    {
                        // Если справка уже существует, обновляем ее
                        string updateQuery = "UPDATE tbl_Docx SET ДатаСправки = @ДатаСправки WHERE Ученик_ID = @Ученик_ID";
                        SqlCommand updateCommand = new SqlCommand(updateQuery, connection);
                        updateCommand.Parameters.AddWithValue("@ДатаСправки", dataPickerDocx.SelectedDate);
                        updateCommand.Parameters.AddWithValue("@Ученик_ID", (int)student.Ученик_ID);

                        int rowsUpdated = updateCommand.ExecuteNonQuery();

                        if (rowsUpdated > 0)
                        {
                            MessageBox.Show("Справка успешно обновлена", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                            ChildWindowClosed?.Invoke(this, EventArgs.Empty);
                            Close();
                        }
                        else
                        {
                            MessageBox.Show("Не удалось обновить справку", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при обработке справки: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ChildWindowClosed?.Invoke(this, EventArgs.Empty);
            Close();
        }
    }
}
