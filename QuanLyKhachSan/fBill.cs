﻿using QuanLyKhachSan.DAO;
using QuanLyKhachSan.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyKhachSan
{
    public partial class fBill : Form
    {
        public fBill()
        {
            InitializeComponent();
            panel1.Visible = false;
        }

        void ShowInfo(int id_bill, int id_checkin)
        {
            //lvBill.Items.Clear();
            List<infoBill> listBillInfo = infoBillDAO.Instance.GetListBillInfo(id_bill, id_checkin);
            float bill_price = Int32.Parse(txbTotalMoney.Text);
            foreach (infoBill item in listBillInfo)
            {
                ListViewItem lvItem = new ListViewItem(item.Id_room.ToString());
                lvItem.SubItems.Add(item.Date_number.ToString());
                lvItem.SubItems.Add(item.Price.ToString());
                lvItem.SubItems.Add(item.Type_ratio.ToString());
                lvItem.SubItems.Add(item.Number_ratio.ToString());
                lvItem.SubItems.Add(item.Total_price.ToString());
                infoBillDAO.Instance.updateMoneyCheckin(item.Total_price, id_checkin);
                bill_price += item.Total_price;
                lvBill.Items.Add(lvItem);
            }
            txbTotalMoney.Text = bill_price.ToString();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (infoBillDAO.Instance.insertBill(txbName.Text, txbAddress.Text))
            {
                MessageBox.Show("Tạo thành công, vui lòng nhập mã phiếu của phòng cần thanh toán");
                lvBill.Items.Clear();
                panel1.Visible = true;
            }
            else
            {
                MessageBox.Show("Có lỗi khi tạo hóa đơn");
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            int id_bill = infoBillDAO.Instance.GetMaxIDBill();
            int id_checkin = int.Parse(txbIDCheckin.Text.ToString());
            int amount_surchage = infoBillDAO.Instance.GetAmountSurchage();
            int number_customer = infoBillDAO.Instance.GetNumberCustomer(id_checkin);
            float ratio = infoBillDAO.Instance.GetRatioCustomer();
            DateTime date_start = infoBillDAO.Instance.GetDate(id_checkin);
            TimeSpan time = DateTime.Now - date_start;
            int date_number = time.Days + 1;
            if (infoBillDAO.Instance.updateBillInfo(id_bill, id_checkin, amount_surchage, number_customer, ratio, date_number))
            {
                ShowInfo(id_bill, id_checkin);
                MessageBox.Show("Thêm phòng thành công");
            }
            else
            {
                MessageBox.Show("Thêm thất bại");
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            int id_checkin = int.Parse(txbIDCheckin.Text.ToString());
            string id_room = infoBillDAO.Instance.GetIdRoom(id_checkin);
            if (HomeDAO.Instance.updateHomeByCreateBill(id_room))
            {
                MessageBox.Show("Xuất hóa đơn thành công");
                panel1.Visible = false;
                this.Close();
            }
            else
            {
                MessageBox.Show("Có lỗi khi xuất hóa đơn");
            }
        }
    }
}
