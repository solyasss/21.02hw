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
        private firm_kanc_tovarov_context _context;

        public main_window()
        {
            InitializeComponent();
            _context = new firm_kanc_tovarov_context();
            load_stationery_types();
            load_type_combo_for_new_stationery();
        }
        private void load_stationery_types()
        {
            var types = _context.StationeryTypes.ToList();
            combo_stationery_types.ItemsSource = types;
        }
        private void load_type_combo_for_new_stationery()
        {
            var types = _context.StationeryTypes.ToList();
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
            var query = from sd in _context.SaleDetails
                        join s in _context.Sales on sd.SaleID equals s.SaleID
                        join st in _context.Stationeries on sd.StationeryID equals st.StationeryID
                        where s.ManagerID == managerId
                        select new
                        {
                            st.StationeryID,
                            st.Name,
                            st.Quantity,
                            st.CostPrice,
                            SoldQty = sd.QuantitySold,
                            s.ManagerID
                        };
            data_grid_main.ItemsSource = query.ToList();
        }
        
        private void btn_show_stationery_by_firm_click(object sender, RoutedEventArgs e)
        {
            int firmId = 2;
            var query = from sd in _context.SaleDetails
                        join s in _context.Sales on sd.SaleID equals s.SaleID
                        join st in _context.Stationeries on sd.StationeryID equals st.StationeryID
                        where s.FirmID == firmId
                        select new
                        {
                            st.StationeryID,
                            st.Name,
                            st.Quantity,
                            st.CostPrice,
                            SoldQty = sd.QuantitySold,
                            s.FirmID
                        };
            data_grid_main.ItemsSource = query.ToList();
        }
        private void btn_show_last_sale_click(object sender, RoutedEventArgs e)
        {
            var lastSaleDate = _context.Sales.Max(x => x.SaleDate);
            var lastSale = _context.Sales.Where(x => x.SaleDate == lastSaleDate).ToList();
            data_grid_main.ItemsSource = lastSale;
        }
        
        private void btn_show_avg_qty_by_type_click(object sender, RoutedEventArgs e)
        {
            var query = from st in _context.Stationeries
                        group st by st.TypeID into g
                        select new
                        {
                            TypeID = g.Key,
                            AvgQty = g.Average(x => x.Quantity)
                        };
            data_grid_main.ItemsSource = query.ToList();
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
            
            var newStationery = new Stationery
            {
                Name = newName,
                TypeID = typeId,
                Quantity = newQuantity,
                CostPrice = newCost
            };
            
            _context.Stationeries.Add(newStationery);
            _context.SaveChanges();

            MessageBox.Show($"Product added (ID={newStationery.StationeryID})");
            
            btn_show_all_stationery_click(null, null);
        }
        
        private void btn_update_stationery_click(object sender, RoutedEventArgs e)
        {
            var item = _context.Stationeries.FirstOrDefault(x => x.StationeryID == 1);
            if (item != null)
            {
                item.Quantity = 999;
                _context.SaveChanges();
                MessageBox.Show("Quantity changed for stationery");
            }
            else
            {
                MessageBox.Show("Product not found");
            }
        }
        
        private void btn_delete_stationery_click(object sender, RoutedEventArgs e)
        {
            var selectedItem = data_grid_main.SelectedItem as Stationery;

            if (selectedItem == null)
            {
                MessageBox.Show("Choose product to delete");
                return;
            }
            
            _context.Stationeries.Remove(selectedItem);
            _context.SaveChanges();

            MessageBox.Show($"Product '{selectedItem.Name}' (ID={selectedItem.StationeryID}) deleted");
            
            btn_show_all_stationery_click(null, null);
        }
        
        private void btn_show_all_types_click(object sender, RoutedEventArgs e)
        {
            var types = _context.StationeryTypes.ToList();
            data_grid_main.ItemsSource = types;
        }
        
        private void btn_show_max_quantity_click(object sender, RoutedEventArgs e)
        {
            int maxQty = _context.Stationeries.Max(s => s.Quantity);
            var maxItems = _context.Stationeries.Where(s => s.Quantity == maxQty).ToList();
            data_grid_main.ItemsSource = maxItems;
        }
        
        private void btn_show_min_quantity_click(object sender, RoutedEventArgs e)
        {
            int minQty = _context.Stationeries.Min(s => s.Quantity);
            var minItems = _context.Stationeries.Where(s => s.Quantity == minQty).ToList();
            data_grid_main.ItemsSource = minItems;
        }
        
        private void btn_show_min_cost_click(object sender, RoutedEventArgs e)
        {
            decimal minCost = _context.Stationeries.Min(s => s.CostPrice);
            var minCostItems = _context.Stationeries.Where(s => s.CostPrice == minCost).ToList();
            data_grid_main.ItemsSource = minCostItems;
        }

        private void btn_show_max_cost_click(object sender, RoutedEventArgs e)
        {
            decimal maxCost = _context.Stationeries.Max(s => s.CostPrice);
            var maxCostItems = _context.Stationeries.Where(s => s.CostPrice == maxCost).ToList();
            data_grid_main.ItemsSource = maxCostItems;
        }
    }
}
