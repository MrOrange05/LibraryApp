using System;
using System.Configuration;
using System.Data;
using System.Windows.Forms;

namespace LibraryApp
{
    public class DatabaseHelper
    {
        public DataTable ExecuteQuery(string query)
        {
            // Тестовые данные для демонстрации
            DataTable table = new DataTable();

            if (query.Contains("Книги"))
            {
                table.Columns.Add("id книги");
                table.Columns.Add("название книги");
                table.Columns.Add("автор книги");

                table.Rows.Add(1, "Бесы", "Достоевский");
                table.Rows.Add(2, "Война и Мир", "Толстой");
                table.Rows.Add(3, "Десять негритят", "Кристи Агата");
            }
            else
            {
                table.Columns.Add("Результат");
                table.Rows.Add("Запрос выполнен: " + query);
            }

            return table;
        }

        public DataSet GetData(string tableName)
        {
            DataSet dataSet = new DataSet();
            DataTable table = new DataTable(tableName);

            if (tableName == "Клиент")
            {
                table.Columns.Add("id клиента");
                table.Columns.Add("адрес библиотеки");
                table.Columns.Add("количество книг на руках");

                table.Rows.Add(1, "Куконковых 10", 2);
                table.Rows.Add(2, "Ленина 15", 6);
                table.Rows.Add(3, "Смирнова 3", 3);
            }
            else if (tableName == "Книги")
            {
                table.Columns.Add("id книги");
                table.Columns.Add("название книги");
                table.Columns.Add("автор книги");

                table.Rows.Add(1, "Бесы", "Достоевский");
                table.Rows.Add(2, "Война и Мир", "Толстой");
                table.Rows.Add(3, "Десять негритят", "Кристи Агата");
            }
            else
            {
                table.Columns.Add("Информация");
                table.Rows.Add("Таблица: " + tableName);
            }

            dataSet.Tables.Add(table);
            return dataSet;
        }

        public void UpdateData(DataTable dataTable, string tableName)
        {
            MessageBox.Show($"Успешно сохранено {dataTable.Rows.Count} записей в таблице {tableName}");
        }

        public DataTable ExecuteStoredProcedure(string procedureName, object parameter)
        {
            DataTable table = new DataTable();
            table.Columns.Add("Книга");
            table.Columns.Add("Автор");
            table.Columns.Add("Результат");

            table.Rows.Add("Бесы", "Достоевский", "Найдено по автору");
            table.Rows.Add("Идиот", "Достоевский", "Найдено по автору");

            return table;
        }
    }
}