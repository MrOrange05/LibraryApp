using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace LibraryApp
{
    public partial class Form1 : Form
    {
        private DatabaseHelper dbHelper;
        private DataSet libraryDataSet;

        // Объявляем все элементы которые будем создавать
        private Button btnLoadData;
        private Button btnSave;
        private Button btnExecuteQuery;
        private Button btnExecuteSP;
        private TextBox txtQuery;
        private DataGridView dgvClients;
        private DataGridView dgvBooks;
        private DataGridView dgvRawQuery;
        private DataGridView dgvStoredProcedure;
        private TabControl tabControlMain;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private TabPage tabPage3;
        private TabPage tabPage4;
        private Panel panelButtons; // Добавим панель для кнопок

        public Form1()
        {
            InitializeComponent();
            CreateControls(); // Создаем элементы управления
            dbHelper = new DatabaseHelper();
            libraryDataSet = new DataSet();
        }

        private void CreateControls()
        {
            // Создаем панель для кнопок вверху
            panelButtons = new Panel();
            panelButtons.Dock = DockStyle.Top;
            panelButtons.Height = 50;
            panelButtons.BackColor = Color.LightGray;
            this.Controls.Add(panelButtons);

            // Создаем кнопки на панели
            btnLoadData = new Button();
            btnLoadData.Text = "Загрузить данные";
            btnLoadData.Location = new Point(10, 10);
            btnLoadData.Size = new Size(120, 30);
            btnLoadData.Click += btnLoadData_Click;
            panelButtons.Controls.Add(btnLoadData);

            btnSave = new Button();
            btnSave.Text = "Сохранить изменения";
            btnSave.Location = new Point(140, 10);
            btnSave.Size = new Size(120, 30);
            btnSave.Click += btnSave_Click;
            panelButtons.Controls.Add(btnSave);

            // Создаем TabControl под панелью с кнопками
            tabControlMain = new TabControl();
            tabControlMain.Dock = DockStyle.Fill;
            tabControlMain.Location = new Point(0, 50);
            tabControlMain.Size = new Size(this.Width, this.Height - 50);
            this.Controls.Add(tabControlMain);

            // Создаем вкладки
            tabPage1 = new TabPage();
            tabPage2 = new TabPage();
            tabPage3 = new TabPage();
            tabPage4 = new TabPage();

            // Настраиваем текст вкладок
            tabPage1.Text = "Клиенты";
            tabPage2.Text = "Книги";
            tabPage3.Text = "SQL Запрос";
            tabPage4.Text = "Хранимая процедура";

            // Добавляем вкладки в TabControl
            tabControlMain.TabPages.Add(tabPage1);
            tabControlMain.TabPages.Add(tabPage2);
            tabControlMain.TabPages.Add(tabPage3);
            tabControlMain.TabPages.Add(tabPage4);

            // Создаем DataGridView для каждой вкладки
            dgvClients = new DataGridView();
            dgvClients.Dock = DockStyle.Fill;
            dgvClients.Name = "dgvClients";
            tabPage1.Controls.Add(dgvClients);

            dgvBooks = new DataGridView();
            dgvBooks.Dock = DockStyle.Fill;
            dgvBooks.Name = "dgvBooks";
            tabPage2.Controls.Add(dgvBooks);

            dgvRawQuery = new DataGridView();
            dgvRawQuery.Dock = DockStyle.Fill;
            dgvRawQuery.Name = "dgvRawQuery";
            tabPage3.Controls.Add(dgvRawQuery);

            dgvStoredProcedure = new DataGridView();
            dgvStoredProcedure.Dock = DockStyle.Fill;
            dgvStoredProcedure.Name = "dgvStoredProcedure";
            tabPage4.Controls.Add(dgvStoredProcedure);

            // Создаем элементы для вкладки SQL Запрос
            txtQuery = new TextBox();
            txtQuery.Multiline = true;
            txtQuery.Dock = DockStyle.Top;
            txtQuery.Height = 100;
            txtQuery.Name = "txtQuery";
            tabPage3.Controls.Add(txtQuery);

            btnExecuteQuery = new Button();
            btnExecuteQuery.Text = "Выполнить запрос";
            btnExecuteQuery.Dock = DockStyle.Top;
            btnExecuteQuery.Height = 30;
            btnExecuteQuery.Click += btnExecuteQuery_Click;
            tabPage3.Controls.Add(btnExecuteQuery);

            // Создаем кнопку для вкладки Хранимая процедура
            btnExecuteSP = new Button();
            btnExecuteSP.Text = "Выполнить процедуру";
            btnExecuteSP.Dock = DockStyle.Top;
            btnExecuteSP.Height = 30;
            btnExecuteSP.Click += btnExecuteSP_Click;
            tabPage4.Controls.Add(btnExecuteSP);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            txtQuery.Text = "SELECT \"название книги\", \"автор книги\" FROM \"Книги\"";
        }

        private void btnLoadData_Click(object sender, EventArgs e)
        {
            try
            {
                // Загрузка данных в отключенном режиме
                var clientsTable = dbHelper.GetData("Клиент").Tables[0];
                var booksTable = dbHelper.GetData("Книги").Tables[0];

                libraryDataSet.Clear();
                libraryDataSet.Tables.Add(clientsTable.Copy());
                libraryDataSet.Tables.Add(booksTable.Copy());

                dgvClients.DataSource = libraryDataSet.Tables[0];
                dgvBooks.DataSource = libraryDataSet.Tables[1];

                MessageBox.Show("Данные успешно загружены!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки: {ex.Message}");
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (libraryDataSet.Tables.Count > 0)
                {
                    dbHelper.UpdateData(libraryDataSet.Tables[0], "Клиент");
                    dbHelper.UpdateData(libraryDataSet.Tables[1], "Книги");
                    MessageBox.Show("Данные успешно сохранены!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения: {ex.Message}");
            }
        }

        private void btnExecuteQuery_Click(object sender, EventArgs e)
        {
            try
            {
                var result = dbHelper.ExecuteQuery(txtQuery.Text);
                dgvRawQuery.DataSource = result;
                MessageBox.Show($"Найдено записей: {result.Rows.Count}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка запроса: {ex.Message}");
            }
        }

        private void btnExecuteSP_Click(object sender, EventArgs e)
        {
            try
            {
                // Создаем параметр для хранимой процедуры
                var parameter = new { Value = "Достоевский" };
                var result = dbHelper.ExecuteStoredProcedure("get_books_by_author", parameter);
                dgvStoredProcedure.DataSource = result;
                MessageBox.Show($"Найдено книг: {result.Rows.Count}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка процедуры: {ex.Message}");
            }
        }

        // CRUD операции - удаление записей
        private void dgvClients_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete && dgvClients.CurrentRow != null)
            {
                if (MessageBox.Show("Удалить запись?", "Подтверждение",
                    MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    dgvClients.Rows.RemoveAt(dgvClients.CurrentRow.Index);
                }
            }
        }

        private void dgvBooks_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete && dgvBooks.CurrentRow != null)
            {
                if (MessageBox.Show("Удалить запись?", "Подтверждение",
                    MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    dgvBooks.Rows.RemoveAt(dgvBooks.CurrentRow.Index);
                }
            }
        }

        // Обработчик изменения размера формы
        private void Form1_Resize(object sender, EventArgs e)
        {
            if (tabControlMain != null)
            {
                tabControlMain.Size = new Size(this.Width, this.Height - 50);
            }
        }
    }
}