using CoilTrackingLocationManagementSystem.Models;

namespace CoilTrackingLocationManagementSystem.Services;

public interface ITrackingRepository
{
    DashboardViewModel GetDashboard();
    MastersViewModel GetMasters();
    TransactionsViewModel GetTransactions();
    ReportsViewModel GetReports();
    IReadOnlyList<Warehouse> GetWarehouses();
    IReadOnlyList<Bay> GetBays();
    IReadOnlyList<SubBay> GetSubBays();
    IReadOnlyList<StorageLocation> GetRows();
    IReadOnlyList<string> GetGrades();
    IReadOnlyList<string> GetSuppliers();
    void CreateInwardEntry(InwardEntryInputModel input);
    void CreateWarehouse(WarehouseInputModel input);
    void UpdateWarehouse(WarehouseInputModel input);
    void DeleteWarehouse(int id);
    void CreateBay(BayInputModel input);
    void UpdateBay(BayInputModel input);
    void DeleteBay(int id);
    void CreateSubBay(SubBayInputModel input);
    void UpdateSubBay(SubBayInputModel input);
    void DeleteSubBay(int id);
    void CreateRow(RowInputModel input);
    void UpdateRow(RowInputModel input);
    void DeleteRow(int id);
}
