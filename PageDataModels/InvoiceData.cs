using InvoiceHandler.Data;

namespace InvoiceHandler.PageDataModels
{
    class InvoiceData
    {
        public List<Customer>? CustomerList { get; set; }
        public DateTime CurrentDate { get; set; }
        public int nextID { get; set; }

        public InvoiceData()
        {
            LoadInvoiceData();
        }

        private void LoadInvoiceData()
        {
            CurrentDate = DateTime.Now;
            CustomerList = dbActions.CustomersToList();
            nextID = dbActions.GetNextInvoiceID();
        }
    }
}
