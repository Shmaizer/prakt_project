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
            dataPickerDocxFirst.SelectedDate = (student.ДатаОкончания != "отсутствует") ? DateTime.Parse(student.ДатаВыдачи) : DateTime.Now;
            dataPickerDockEnd.SelectedDate = (student.ДатаОкончания != "отсутствует") ? DateTime.Parse(student.ДатаОкончания) : DateTime.Parse($"{(int)DateTime.Now.Day+1}.{DateTime.Now.Month}.{DateTime.Now.Year}");
        }
        

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string checkQuery = "SELECT Документ_ID FROM tbl_Docx WHERE Ученик_ID = @Ученик_ID";
                    SqlCommand checkCommand = new SqlCommand(checkQuery, connection);
                    checkCommand.Parameters.AddWithValue("@Ученик_ID", student.Ученик_ID);

                    object docId = checkCommand.ExecuteScalar();

                    if (docId == null)
                    {
                        string insertQuery = "INSERT INTO tbl_Docx (ДатаВыдачи, ДатаОкончания,Ученик_ID) VALUES (@ДатаВыдачи,@ДатаОкончания, @Ученик_ID)";
                        SqlCommand insertCommand = new SqlCommand(insertQuery, connection);
                        insertCommand.Parameters.AddWithValue("@ДатаВыдачи", dataPickerDocxFirst.SelectedDate);
                        insertCommand.Parameters.AddWithValue("@ДатаОкончания", dataPickerDockEnd.SelectedDate);
                        insertCommand.Parameters.AddWithValue("@Ученик_ID", (int)student.Ученик_ID);

                        int rowsInserted = insertCommand.ExecuteNonQuery();

                        if (rowsInserted > 0)
                        {
                            MessageBox.Show("Справка успешно обновлена", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                            ChildWindowClosed?.Invoke(this, EventArgs.Empty);
                            Close();
                        }
                        else
                        {
                            MessageBox.Show("Не удалось создать справку", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    else
                    {
                        string updateQuery = "UPDATE tbl_Docx SET ДатаВыдачи = @ДатаВыдачи, ДатаОкончания = @ДатаОкончания WHERE Ученик_ID = @Ученик_ID";
                        SqlCommand updateCommand = new SqlCommand(updateQuery, connection);
                        updateCommand.Parameters.AddWithValue("@ДатаВыдачи", dataPickerDocxFirst.SelectedDate);
                        updateCommand.Parameters.AddWithValue("@ДатаОкончания", dataPickerDockEnd.SelectedDate);
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
        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            bool isValidInput = dataPickerDocxFirst.SelectedDate.HasValue &&
                                dataPickerDockEnd.SelectedDate.HasValue &&
                                dataPickerDocxFirst.SelectedDate <= dataPickerDockEnd.SelectedDate;
            buttonOK.IsEnabled = isValidInput;
            dataPickerDocxFirst.Background = isValidInput ? System.Windows.Media.Brushes.White : System.Windows.Media.Brushes.LightCoral;
            dataPickerDockEnd.Background = isValidInput ? System.Windows.Media.Brushes.White : System.Windows.Media.Brushes.LightCoral;
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ChildWindowClosed?.Invoke(this, EventArgs.Empty);
            Close();
        }
    }
}
