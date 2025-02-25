using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;
using _21._02hw.Models;

namespace hw
{
    public partial class main_window : Window
    { 
        private FirmKancTovarovContext _context;

        public main_window()
        {
            InitializeComponent();
            _context = new FirmKancTovarovContext();
            load_stationery_types();
            load_type_combo_for_new_stationery();
        }

        private void load_stationery_types()
        {
            var types = _context.StationeryTypes.FromSqlRaw("EXEC sp_get_all_types").ToList();
            combo_stationery_types.ItemsSource = types;
        }

        private void load_type_combo_for_new_stationery()
        {
            var types = _context.StationeryTypes.FromSqlRaw("EXEC sp_get_all_types").ToList();
            cbNewStationeryType.ItemsSource = types;
        }

        private void combo_stationery_types_selection_changed(object sender, SelectionChangedEventArgs e)
        {
            if (combo_stationery_types.SelectedValue != null)
            {
                int selected_type_id = (int)combo_stationery_types.SelectedValue;
                var stationery_by_type = _context.Stationeries
                    .FromSqlRaw("EXEC sp_get_stationery_by_type @type_id = {0}", selected_type_id)
                    .ToList();
                data_grid_main.ItemsSource = stationery_by_type;
            }
        }

        private void btn_show_all_stationery_click(object sender, RoutedEventArgs e)
        {
            var all_stationery = _context.Stationeries
                .FromSqlRaw("EXEC sp_get_all_stationery")
                .ToList();
            data_grid_main.ItemsSource = all_stationery;
        }

        private void btn_show_all_managers_click(object sender, RoutedEventArgs e)
        {
            var all_managers = _context.Managers
                .FromSqlRaw("EXEC sp_get_all_managers")
                .ToList();
            data_grid_main.ItemsSource = all_managers;
        }

        private void btn_show_stationery_by_manager_click(object sender, RoutedEventArgs e)
        {
            int managerId = 1;
            var stationery_by_manager = _context.Stationeries
                .FromSqlRaw("EXEC sp_get_stationery_by_manager @manager_id = {0}", managerId)
                .ToList();
            data_grid_main.ItemsSource = stationery_by_manager;
        }

        private void btn_show_stationery_by_firm_click(object sender, RoutedEventArgs e)
        {
            int firmId = 2;
            var stationery_by_firm = _context.Stationeries
                .FromSqlRaw("EXEC sp_get_stationery_by_firm @firm_id = {0}", firmId)
                .ToList();
            data_grid_main.ItemsSource = stationery_by_firm;
        }

        private void btn_show_last_sale_click(object sender, RoutedEventArgs e)
        {
            var last_sale = _context.Sales
                .FromSqlRaw("EXEC sp_get_last_sale")
                .ToList();
            data_grid_main.ItemsSource = last_sale;
        }
        
            private void btn_show_avg_qty_by_type_click(object sender, RoutedEventArgs e)
            {
                var avgQuantityByType = _context.Set<AvgQuantityByTypeResult>()
                    .FromSqlRaw("EXEC sp_get_avg_quantity_by_type")
                    .ToList();
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

            _context.Database.ExecuteSqlRaw("EXEC sp_insert_stationery @name = {0}, @type_id = {1}, @quantity = {2}, @cost_price = {3}", 
                newName, typeId, newQuantity, newCost);

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

            _context.Database.ExecuteSqlRaw("EXEC sp_update_stationery @stationery_id = {0}, @quantity = {1}", 
                selectedItem.StationeryId, 999);

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

            _context.Database.ExecuteSqlRaw("EXEC sp_delete_stationery @stationery_id = {0}", selectedItem.StationeryId);

            MessageBox.Show($"Product '{selectedItem.Name}' (ID={selectedItem.StationeryId}) deleted");
            btn_show_all_stationery_click(null, null);
        }

        private void btn_show_all_types_click(object sender, RoutedEventArgs e)
        {
            var types = _context.StationeryTypes.FromSqlRaw("EXEC sp_get_all_types").ToList();
            data_grid_main.ItemsSource = types;
        }

        private void btn_show_max_quantity_click(object sender, RoutedEventArgs e)
        {
            var max_quantity_stationery = _context.Stationeries
                .FromSqlRaw("EXEC sp_get_max_quantity_stationery")
                .ToList();
            data_grid_main.ItemsSource = max_quantity_stationery;
        }

        private void btn_show_min_quantity_click(object sender, RoutedEventArgs e)
        {
            var min_quantity_stationery = _context.Stationeries
                .FromSqlRaw("EXEC sp_get_min_quantity_stationery")
                .ToList();
            data_grid_main.ItemsSource = min_quantity_stationery;
        }

        private void btn_show_min_cost_click(object sender, RoutedEventArgs e)
        {
            var min_cost_stationery = _context.Stationeries
                .FromSqlRaw("EXEC sp_get_min_cost_stationery")
                .ToList();
            data_grid_main.ItemsSource = min_cost_stationery;
        }

        private void btn_show_max_cost_click(object sender, RoutedEventArgs e)
        {
            var max_cost_stationery = _context.Stationeries
                .FromSqlRaw("EXEC sp_get_max_cost_stationery")
                .ToList();
            data_grid_main.ItemsSource = max_cost_stationery;
        }
    }
}