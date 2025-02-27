using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Dapper;
using _21._02hw.Models;

namespace hw
{
    public partial class main_window : Window
    {
        private DapperContext _context;

        public main_window()
        {
            InitializeComponent();
            _context = new DapperContext("Server=DESKTOP-IB673J5\\SQLEXPRESS;Database=FirmKancTovarov;Trusted_Connection=True;TrustServerCertificate=True;");
            load_stationery_types();
            load_type_combo_for_new_stationery();
        }

        private void load_stationery_types()
        {
            var types = _context.Query<StationeryType>("EXEC sp_get_all_types");
            combo_stationery_types.ItemsSource = types;
        }

        private void load_type_combo_for_new_stationery()
        {
            var types = _context.Query<StationeryType>("EXEC sp_get_all_types");
            cbNewStationeryType.ItemsSource = types;
        }

        private void combo_stationery_types_selection_changed(object sender, SelectionChangedEventArgs e)
        {
            if (combo_stationery_types.SelectedValue != null)
            {
                int selected_type_id = (int)combo_stationery_types.SelectedValue;
                var stationery_by_type = _context.Query<Stationery>("EXEC sp_get_stationery_by_type @type_id = @typeId", new { typeId = selected_type_id });
                data_grid_main.ItemsSource = stationery_by_type;
            }
        }

        private void btn_show_all_stationery_click(object sender, RoutedEventArgs e)
        {
            var all_stationery = _context.Query<Stationery>("EXEC sp_get_all_stationery");
            data_grid_main.ItemsSource = all_stationery;
        }

        private void btn_show_all_managers_click(object sender, RoutedEventArgs e)
        {
            var all_managers = _context.Query<Manager>("EXEC sp_get_all_managers");
            data_grid_main.ItemsSource = all_managers;
        }

        private void btn_show_stationery_by_manager_click(object sender, RoutedEventArgs e)
        {
            int managerId = 1;
            var stationery_by_manager = _context.Query<Stationery>("EXEC sp_get_stationery_by_manager @manager_id = @managerId", new { managerId });
            data_grid_main.ItemsSource = stationery_by_manager;
        }

        private void btn_show_stationery_by_firm_click(object sender, RoutedEventArgs e)
        {
            int firmId = 2;
            var stationery_by_firm = _context.Query<Stationery>("EXEC sp_get_stationery_by_firm @firm_id = @firmId", new { firmId });
            data_grid_main.ItemsSource = stationery_by_firm;
        }

        private void btn_show_last_sale_click(object sender, RoutedEventArgs e)
        {
            var last_sale = _context.Query<Sale>("EXEC sp_get_last_sale");
            data_grid_main.ItemsSource = last_sale;
        }

        private void btn_show_avg_qty_by_type_click(object sender, RoutedEventArgs e)
        {
            var avgQuantityByType = _context.Query<AvgQuantityByTypeResult>("EXEC sp_get_avg_quantity_by_type");
            data_grid_main.ItemsSource = avgQuantityByType;
        }

        private void btn_add_stationery_click(object sender, RoutedEventArgs e)
        {
            string newName = tbNewStationeryName.Text.Trim();

            if (string.IsNullOrWhiteSpace(newName))
            {
                MessageBox.Show("Enter product name");
                return;
            }

            if (!int.TryParse(tbNewStationeryQuantity.Text.Trim(), out int newQuantity))
            {
                MessageBox.Show("Invalid quantity");
                return;
            }

            if (!decimal.TryParse(tbNewStationeryCostPrice.Text.Trim(), out decimal newCost))
            {
                MessageBox.Show("Invalid cost");
                return;
            }

            if (cbNewStationeryType.SelectedValue == null)
            {
                MessageBox.Show("Choose a stationery type");
                return;
            }

            int typeId = (int)cbNewStationeryType.SelectedValue;

            _context.Execute("EXEC sp_insert_stationery @name = @name, @type_id = @typeId, @quantity = @quantity, @cost_price = @costPrice",
                new { name = newName, typeId, quantity = newQuantity, costPrice = newCost });

            MessageBox.Show("Product added");
            btn_show_all_stationery_click(null, null);
        }

        private void btn_update_stationery_click(object sender, RoutedEventArgs e)
        {
            var selectedItem = data_grid_main.SelectedItem as Stationery;

            if (selectedItem == null)
            {
                MessageBox.Show("Choose product to update");
                return;
            }

            _context.Execute("EXEC sp_update_stationery @stationery_id = @stationeryId, @quantity = @quantity",
                new { stationeryId = selectedItem.StationeryId, quantity = 999 });

            MessageBox.Show("Quantity changed for stationery");
            btn_show_all_stationery_click(null, null);
        }

        private void btn_delete_stationery_click(object sender, RoutedEventArgs e)
        {
            var selectedItem = data_grid_main.SelectedItem as Stationery;

            if (selectedItem == null)
            {
                MessageBox.Show("Choose product to delete");
                return;
            }

            _context.Execute("EXEC sp_delete_stationery @stationery_id = @stationeryId", new { stationeryId = selectedItem.StationeryId });

            MessageBox.Show($"Product '{selectedItem.Name}' (ID={selectedItem.StationeryId}) deleted");
            btn_show_all_stationery_click(null, null);
        }

        private void btn_show_all_types_click(object sender, RoutedEventArgs e)
        {
            var types = _context.Query<StationeryType>("EXEC sp_get_all_types");
            data_grid_main.ItemsSource = types;
        }

        private void btn_show_max_quantity_click(object sender, RoutedEventArgs e)
        {
            var max_quantity_stationery = _context.Query<Stationery>("EXEC sp_get_max_quantity_stationery");
            data_grid_main.ItemsSource = max_quantity_stationery;
        }

        private void btn_show_min_quantity_click(object sender, RoutedEventArgs e)
        {
            var min_quantity_stationery = _context.Query<Stationery>("EXEC sp_get_min_quantity_stationery");
            data_grid_main.ItemsSource = min_quantity_stationery;
        }

        private void btn_show_min_cost_click(object sender, RoutedEventArgs e)
        {
            var min_cost_stationery = _context.Query<Stationery>("EXEC sp_get_min_cost_stationery");
            data_grid_main.ItemsSource = min_cost_stationery;
        }

        private void btn_show_max_cost_click(object sender, RoutedEventArgs e)
        {
            var max_cost_stationery = _context.Query<Stationery>("EXEC sp_get_max_cost_stationery");
            data_grid_main.ItemsSource = max_cost_stationery;
        }
    }
}