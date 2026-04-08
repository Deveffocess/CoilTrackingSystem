using CoilTrackingLocationManagementSystem.Models;

namespace CoilTrackingLocationManagementSystem.Services;

public class InMemoryTrackingRepository : ITrackingRepository
{
    private readonly object _syncRoot = new();

    private readonly List<Warehouse> _warehouses =
    [
        new Warehouse { Id = 1, Code = "WH-01", Name = "Main Warehouse", Description = "Primary coil storage warehouse", IsActive = true, CreatedDate = DateTime.Today.AddDays(-7).AddHours(9), ModifiedDate = DateTime.Today.AddDays(-2).AddHours(10) },
        new Warehouse { Id = 2, Code = "WH-02", Name = "Dispatch Warehouse", Description = "Finished material and dispatch holding area", IsActive = true, CreatedDate = DateTime.Today.AddDays(-6).AddHours(11), ModifiedDate = DateTime.Today.AddDays(-1).AddHours(16) }
    ];

    private readonly List<Bay> _bays =
    [
        new Bay { Id = 1, WarehouseCode = "WH-01", Code = "BAY-A", Name = "Bay A", IsActive = true, CreatedDate = DateTime.Today.AddDays(-6).AddHours(9), ModifiedDate = DateTime.Today.AddDays(-2).AddHours(12) },
        new Bay { Id = 2, WarehouseCode = "WH-01", Code = "BAY-B", Name = "Bay B", IsActive = true, CreatedDate = DateTime.Today.AddDays(-5).AddHours(10), ModifiedDate = DateTime.Today.AddDays(-1).AddHours(14) },
        new Bay { Id = 3, WarehouseCode = "WH-02", Code = "BAY-D", Name = "Dispatch Bay", IsActive = true, CreatedDate = DateTime.Today.AddDays(-4).AddHours(8), ModifiedDate = DateTime.Today.AddDays(-1).AddHours(17) }
    ];

    private readonly List<SubBay> _subBays =
    [
        new SubBay { Id = 1, WarehouseCode = "WH-01", BayCode = "BAY-A", Code = "SB-01", Name = "SubBay 01", IsActive = true, CreatedDate = DateTime.Today.AddDays(-5).AddHours(9), ModifiedDate = DateTime.Today.AddDays(-2).AddHours(13) },
        new SubBay { Id = 2, WarehouseCode = "WH-01", BayCode = "BAY-A", Code = "SB-02", Name = "SubBay 02", IsActive = true, CreatedDate = DateTime.Today.AddDays(-5).AddHours(10), ModifiedDate = DateTime.Today.AddDays(-2).AddHours(14) },
        new SubBay { Id = 3, WarehouseCode = "WH-01", BayCode = "BAY-B", Code = "SB-01", Name = "Inspection SubBay", IsActive = true, CreatedDate = DateTime.Today.AddDays(-4).AddHours(11), ModifiedDate = DateTime.Today.AddDays(-1).AddHours(11) },
        new SubBay { Id = 4, WarehouseCode = "WH-02", BayCode = "BAY-D", Code = "SB-01", Name = "Dispatch Staging", IsActive = true, CreatedDate = DateTime.Today.AddDays(-4).AddHours(12), ModifiedDate = DateTime.Today.AddDays(-1).AddHours(18) }
    ];

    private readonly List<StorageLocation> _locations =
    [
        new StorageLocation { Id = 1, Code = "ROW-A-01", Name = "Main Receiving Row 01", Warehouse = "Main Warehouse", Bay = "Bay A", SubBay = "SubBay 01", Row = "Row 01", Capacity = 12, Occupied = 8, IsActive = true, CreatedDate = DateTime.Today.AddDays(-4).AddHours(9), ModifiedDate = DateTime.Today.AddDays(-1).AddHours(9) },
        new StorageLocation { Id = 2, Code = "ROW-A-02", Name = "Main Receiving Row 02", Warehouse = "Main Warehouse", Bay = "Bay A", SubBay = "SubBay 02", Row = "Row 02", Capacity = 12, Occupied = 10, IsActive = true, CreatedDate = DateTime.Today.AddDays(-4).AddHours(10), ModifiedDate = DateTime.Today.AddDays(-1).AddHours(10) },
        new StorageLocation { Id = 3, Code = "ROW-B-01", Name = "Inspection Row 01", Warehouse = "Main Warehouse", Bay = "Bay B", SubBay = "Inspection SubBay", Row = "Row 01", Capacity = 8, Occupied = 3, IsActive = true, CreatedDate = DateTime.Today.AddDays(-3).AddHours(11), ModifiedDate = DateTime.Today.AddDays(-1).AddHours(12) },
        new StorageLocation { Id = 4, Code = "ROW-D-01", Name = "Dispatch Row 01", Warehouse = "Dispatch Warehouse", Bay = "Dispatch Bay", SubBay = "Dispatch Staging", Row = "Row 01", Capacity = 6, Occupied = 4, IsActive = true, CreatedDate = DateTime.Today.AddDays(-3).AddHours(12), ModifiedDate = DateTime.Today.AddDays(-1).AddHours(18) }
    ];

    private readonly List<Coil> _coils =
    [
        new Coil { Id = 1, CoilNumber = "CL-24001", HeatNumber = "H-10021", Grade = "CRCA", Thickness = 1.20m, Width = 1250m, Weight = 15.40m, CurrentLocationCode = "ROW-A-01", Status = CoilStatus.Stored, ReceivedDate = DateTime.Today.AddDays(-3) },
        new Coil { Id = 2, CoilNumber = "CL-24002", HeatNumber = "H-10022", Grade = "HR", Thickness = 2.50m, Width = 1500m, Weight = 18.10m, CurrentLocationCode = "ROW-B-01", Status = CoilStatus.InInspection, ReceivedDate = DateTime.Today.AddDays(-2) },
        new Coil { Id = 3, CoilNumber = "CL-24003", HeatNumber = "H-10023", Grade = "GP", Thickness = 0.80m, Width = 1000m, Weight = 11.35m, CurrentLocationCode = "ROW-D-01", Status = CoilStatus.Issued, ReceivedDate = DateTime.Today.AddDays(-1) },
        new Coil { Id = 4, CoilNumber = "CL-24004", HeatNumber = "H-10024", Grade = "CRFH", Thickness = 1.60m, Width = 1220m, Weight = 14.75m, CurrentLocationCode = "ROW-A-02", Status = CoilStatus.Stored, ReceivedDate = DateTime.Today }
    ];

    private readonly List<CoilMovement> _movements =
    [
        new CoilMovement { Id = 1, CoilNumber = "CL-24001", FromLocation = "Gate In", ToLocation = "Main Warehouse / Bay A / SubBay 01 / Row 01", Activity = "Inward Entry", ReferenceNumber = "GRN-2026-101", UpdatedBy = "Store Admin", MovementDate = DateTime.Today.AddHours(-20) },
        new CoilMovement { Id = 2, CoilNumber = "CL-24002", FromLocation = "Main Warehouse / Bay A / SubBay 01 / Row 01", ToLocation = "Main Warehouse / Bay B / Inspection SubBay / Row 01", Activity = "QC Transfer", ReferenceNumber = "QC-2026-054", UpdatedBy = "QA User", MovementDate = DateTime.Today.AddHours(-12) },
        new CoilMovement { Id = 3, CoilNumber = "CL-24003", FromLocation = "Main Warehouse / Bay B / Inspection SubBay / Row 01", ToLocation = "Dispatch Warehouse / Dispatch Bay / Dispatch Staging / Row 01", Activity = "Dispatch Staging", ReferenceNumber = "DSP-2026-021", UpdatedBy = "Dispatch User", MovementDate = DateTime.Today.AddHours(-4) }
    ];

    public DashboardViewModel GetDashboard()
    {
        return new DashboardViewModel
        {
            TotalCoils = _coils.Count,
            StoredCoils = _coils.Count(x => x.Status == CoilStatus.Stored),
            InspectionPending = _coils.Count(x => x.Status == CoilStatus.InInspection),
            DispatchReady = _coils.Count(x => x.Status == CoilStatus.Issued),
            RecentCoils = _coils
                .OrderByDescending(x => x.ReceivedDate)
                .Select(WithResolvedLocation)
                .ToList(),
            Locations = _locations,
            RecentMovements = _movements.OrderByDescending(x => x.MovementDate).ToList()
        };
    }

    public MastersViewModel GetMasters()
    {
        return new MastersViewModel
        {
            Warehouses = GetWarehouses(),
            Bays = GetBays(),
            SubBays = GetSubBays(),
            Locations = GetRows(),
            Grades = GetGrades(),
            Suppliers = GetSuppliers()
        };
    }

    public TransactionsViewModel GetTransactions()
    {
        return new TransactionsViewModel
        {
            InwardCoils = _coils.OrderByDescending(x => x.ReceivedDate).Select(WithResolvedLocation).ToList(),
            Movements = _movements.OrderByDescending(x => x.MovementDate).ToList(),
            Locations = _locations,
            Grades = ["CRCA", "CRFH", "GP", "HR", "GI"]
        };
    }

    public ReportsViewModel GetReports()
    {
        return new ReportsViewModel
        {
            Coils = _coils.OrderBy(x => x.CoilNumber).Select(WithResolvedLocation).ToList(),
            Movements = _movements.OrderByDescending(x => x.MovementDate).ToList()
        };
    }

    public IReadOnlyList<Warehouse> GetWarehouses() => _warehouses;

    public IReadOnlyList<Bay> GetBays() => _bays;

    public IReadOnlyList<SubBay> GetSubBays() => _subBays;

    public IReadOnlyList<StorageLocation> GetRows() => _locations;

    public IReadOnlyList<string> GetGrades() => ["CRCA", "CRFH", "GP", "HR", "GI"];

    public IReadOnlyList<string> GetSuppliers() => ["Tata Steel", "JSW", "SAIL", "ArcelorMittal"];

    public void CreateInwardEntry(InwardEntryInputModel input)
    {
        lock (_syncRoot)
        {
            var row = _locations.FirstOrDefault(x => x.Id == input.LocationId)
                ?? throw new InvalidOperationException("Selected row does not exist.");

            if (!row.IsActive)
            {
                throw new InvalidOperationException("Selected row is inactive.");
            }

            if (_coils.Any(x => x.CoilNumber.Equals(input.CoilNumber.Trim(), StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidOperationException("Coil number already exists.");
            }

            if (row.Occupied >= row.Capacity)
            {
                throw new InvalidOperationException("Selected row is already full.");
            }

            var now = input.ReceivedDate == default ? DateTime.Now : input.ReceivedDate;
            var coil = new Coil
            {
                Id = NextId(_coils.Select(x => x.Id)),
                CoilNumber = input.CoilNumber.Trim(),
                HeatNumber = input.HeatNumber.Trim(),
                Grade = input.Grade.Trim(),
                Thickness = input.Thickness,
                Width = input.Width,
                Weight = input.Weight,
                CurrentLocationCode = row.Code,
                Status = CoilStatus.Stored,
                ReceivedDate = now
            };

            _coils.Add(coil);

            row.Occupied++;
            row.ModifiedDate = DateTime.Now;

            _movements.Add(new CoilMovement
            {
                Id = NextId(_movements.Select(x => x.Id)),
                CoilNumber = coil.CoilNumber,
                FromLocation = "Gate In",
                ToLocation = row.HierarchyPath,
                Activity = "Inward Entry",
                ReferenceNumber = input.GrnNumber.Trim(),
                UpdatedBy = input.ReceivedBy.Trim(),
                MovementDate = now
            });
        }
    }

    public void CreateWarehouse(WarehouseInputModel input)
    {
        lock (_syncRoot)
        {
            EnsureWarehouseCodeAvailable(input.Code);

            _warehouses.Add(new Warehouse
            {
                Id = NextId(_warehouses.Select(x => x.Id)),
                Code = input.Code.Trim(),
                Name = input.Name.Trim(),
                Description = input.Description.Trim(),
                IsActive = input.IsActive,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now
            });
        }
    }

    public void UpdateWarehouse(WarehouseInputModel input)
    {
        lock (_syncRoot)
        {
            var warehouse = _warehouses.FirstOrDefault(x => x.Id == input.Id)
                ?? throw new InvalidOperationException("Warehouse not found.");

            EnsureWarehouseCodeAvailable(input.Code, input.Id);

            var oldCode = warehouse.Code;
            var oldName = warehouse.Name;
            var newCode = input.Code.Trim();
            var newName = input.Name.Trim();

            warehouse.Code = newCode;
            warehouse.Name = newName;
            warehouse.Description = input.Description.Trim();
            warehouse.IsActive = input.IsActive;
            warehouse.ModifiedDate = DateTime.Now;

            foreach (var bay in _bays.Where(x => x.WarehouseCode == oldCode))
            {
                bay.WarehouseCode = newCode;
            }

            foreach (var subBay in _subBays.Where(x => x.WarehouseCode == oldCode))
            {
                subBay.WarehouseCode = newCode;
            }

            foreach (var row in _locations.Where(x => x.Warehouse == oldName))
            {
                row.Warehouse = newName;
            }
        }
    }

    public void DeleteWarehouse(int id)
    {
        lock (_syncRoot)
        {
            var warehouse = _warehouses.FirstOrDefault(x => x.Id == id)
                ?? throw new InvalidOperationException("Warehouse not found.");

            if (_bays.Any(x => x.WarehouseCode == warehouse.Code))
            {
                throw new InvalidOperationException("Delete bays under this warehouse first.");
            }

            _warehouses.Remove(warehouse);
        }
    }

    public void CreateBay(BayInputModel input)
    {
        lock (_syncRoot)
        {
            EnsureWarehouseExists(input.WarehouseCode);
            EnsureBayCodeAvailable(input.WarehouseCode, input.Code);

            _bays.Add(new Bay
            {
                Id = NextId(_bays.Select(x => x.Id)),
                WarehouseCode = input.WarehouseCode.Trim(),
                Code = input.Code.Trim(),
                Name = input.Name.Trim(),
                IsActive = input.IsActive,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now
            });
        }
    }

    public void UpdateBay(BayInputModel input)
    {
        lock (_syncRoot)
        {
            var bay = _bays.FirstOrDefault(x => x.Id == input.Id)
                ?? throw new InvalidOperationException("Bay not found.");

            EnsureWarehouseExists(input.WarehouseCode);
            EnsureBayCodeAvailable(input.WarehouseCode, input.Code, input.Id);

            var oldCode = bay.Code;
            var oldWarehouseCode = bay.WarehouseCode;
            var oldWarehouseName = GetWarehouseName(oldWarehouseCode);
            var oldBayName = bay.Name;

            bay.WarehouseCode = input.WarehouseCode.Trim();
            bay.Code = input.Code.Trim();
            bay.Name = input.Name.Trim();
            bay.IsActive = input.IsActive;
            bay.ModifiedDate = DateTime.Now;

            var newWarehouseName = GetWarehouseName(bay.WarehouseCode);

            foreach (var subBay in _subBays.Where(x => x.WarehouseCode == oldWarehouseCode && x.BayCode == oldCode))
            {
                subBay.WarehouseCode = bay.WarehouseCode;
                subBay.BayCode = bay.Code;
            }

            foreach (var row in _locations.Where(x => x.Warehouse == oldWarehouseName && x.Bay == oldBayName))
            {
                row.Warehouse = newWarehouseName;
                row.Bay = bay.Name;
            }
        }
    }

    public void DeleteBay(int id)
    {
        lock (_syncRoot)
        {
            var bay = _bays.FirstOrDefault(x => x.Id == id)
                ?? throw new InvalidOperationException("Bay not found.");

            if (_subBays.Any(x => x.WarehouseCode == bay.WarehouseCode && x.BayCode == bay.Code))
            {
                throw new InvalidOperationException("Delete sub-bays under this bay first.");
            }

            _bays.Remove(bay);
        }
    }

    public void CreateSubBay(SubBayInputModel input)
    {
        lock (_syncRoot)
        {
            EnsureWarehouseExists(input.WarehouseCode);
            EnsureBayExists(input.WarehouseCode, input.BayCode);
            EnsureSubBayCodeAvailable(input.BayCode, input.Code);

            _subBays.Add(new SubBay
            {
                Id = NextId(_subBays.Select(x => x.Id)),
                WarehouseCode = input.WarehouseCode.Trim(),
                BayCode = input.BayCode.Trim(),
                Code = input.Code.Trim(),
                Name = input.Name.Trim(),
                IsActive = input.IsActive,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now
            });
        }
    }

    public void UpdateSubBay(SubBayInputModel input)
    {
        lock (_syncRoot)
        {
            var subBay = _subBays.FirstOrDefault(x => x.Id == input.Id)
                ?? throw new InvalidOperationException("Sub-bay not found.");

            EnsureWarehouseExists(input.WarehouseCode);
            EnsureBayExists(input.WarehouseCode, input.BayCode);
            EnsureSubBayCodeAvailable(input.BayCode, input.Code, input.Id);

            var oldWarehouseName = GetWarehouseName(subBay.WarehouseCode);
            var oldBayName = GetBayName(subBay.WarehouseCode, subBay.BayCode);
            var oldSubBayName = subBay.Name;

            subBay.WarehouseCode = input.WarehouseCode.Trim();
            subBay.BayCode = input.BayCode.Trim();
            subBay.Code = input.Code.Trim();
            subBay.Name = input.Name.Trim();
            subBay.IsActive = input.IsActive;
            subBay.ModifiedDate = DateTime.Now;

            var newWarehouseName = GetWarehouseName(subBay.WarehouseCode);
            var newBayName = GetBayName(subBay.WarehouseCode, subBay.BayCode);

            foreach (var row in _locations.Where(x => x.SubBay == oldSubBayName && x.Bay == oldBayName && x.Warehouse == oldWarehouseName))
            {
                row.Warehouse = newWarehouseName;
                row.Bay = newBayName;
                row.SubBay = subBay.Name;
            }

        }
    }

    public void DeleteSubBay(int id)
    {
        lock (_syncRoot)
        {
            var subBay = _subBays.FirstOrDefault(x => x.Id == id)
                ?? throw new InvalidOperationException("Sub-bay not found.");

            if (_locations.Any(x => x.SubBay == subBay.Name && x.Bay == GetBayName(subBay.WarehouseCode, subBay.BayCode)))
            {
                throw new InvalidOperationException("Delete rows under this sub-bay first.");
            }

            _subBays.Remove(subBay);
        }
    }

    public void CreateRow(RowInputModel input)
    {
        lock (_syncRoot)
        {
            EnsureRowCodeAvailable(input.Code);
            EnsureOccupiedWithinCapacity(input.Occupied, input.Capacity);

            var subBay = _subBays.FirstOrDefault(x => x.Id == input.SubBayId)
                ?? throw new InvalidOperationException("Sub-bay not found.");

            var warehouseName = GetWarehouseName(subBay.WarehouseCode);
            var bayName = GetBayName(subBay.WarehouseCode, subBay.BayCode);

            _locations.Add(new StorageLocation
            {
                Id = NextId(_locations.Select(x => x.Id)),
                Code = input.Code.Trim(),
                Name = input.Name.Trim(),
                Warehouse = warehouseName,
                Bay = bayName,
                SubBay = subBay.Name,
                Row = input.Row.Trim(),
                Capacity = input.Capacity,
                Occupied = input.Occupied,
                IsActive = input.IsActive,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now
            });
        }
    }

    public void UpdateRow(RowInputModel input)
    {
        lock (_syncRoot)
        {
            var row = _locations.FirstOrDefault(x => x.Id == input.Id)
                ?? throw new InvalidOperationException("Row not found.");

            EnsureRowCodeAvailable(input.Code, input.Id);
            EnsureOccupiedWithinCapacity(input.Occupied, input.Capacity);

            var subBay = _subBays.FirstOrDefault(x => x.Id == input.SubBayId)
                ?? throw new InvalidOperationException("Sub-bay not found.");

            var oldCode = row.Code;

            row.Code = input.Code.Trim();
            row.Name = input.Name.Trim();
            row.Warehouse = GetWarehouseName(subBay.WarehouseCode);
            row.Bay = GetBayName(subBay.WarehouseCode, subBay.BayCode);
            row.SubBay = subBay.Name;
            row.Row = input.Row.Trim();
            row.Capacity = input.Capacity;
            row.Occupied = input.Occupied;
            row.IsActive = input.IsActive;
            row.ModifiedDate = DateTime.Now;

            foreach (var coil in _coils.Where(x => x.CurrentLocationCode == oldCode))
            {
                coil.CurrentLocationCode = row.Code;
            }
        }
    }

    public void DeleteRow(int id)
    {
        lock (_syncRoot)
        {
            var row = _locations.FirstOrDefault(x => x.Id == id)
                ?? throw new InvalidOperationException("Row not found.");

            if (_coils.Any(x => x.CurrentLocationCode == row.Code))
            {
                throw new InvalidOperationException("This row is assigned to one or more coils.");
            }

            _locations.Remove(row);
        }
    }

    private Coil WithResolvedLocation(Coil coil)
    {
        var location = _locations.FirstOrDefault(x => x.Code == coil.CurrentLocationCode);
        if (location is null)
        {
            return coil;
        }

        return new Coil
        {
            Id = coil.Id,
            CoilNumber = coil.CoilNumber,
            HeatNumber = coil.HeatNumber,
            Grade = coil.Grade,
            Thickness = coil.Thickness,
            Width = coil.Width,
            Weight = coil.Weight,
            CurrentLocationCode = $"{location.Code} ({location.HierarchyPath})",
            Status = coil.Status,
            ReceivedDate = coil.ReceivedDate
        };
    }

    private void EnsureWarehouseExists(string warehouseCode)
    {
        if (!_warehouses.Any(x => x.Code.Equals(warehouseCode.Trim(), StringComparison.OrdinalIgnoreCase)))
        {
            throw new InvalidOperationException("Selected warehouse does not exist.");
        }
    }

    private void EnsureBayExists(string warehouseCode, string bayCode)
    {
        if (!_bays.Any(x =>
                x.WarehouseCode.Equals(warehouseCode.Trim(), StringComparison.OrdinalIgnoreCase) &&
                x.Code.Equals(bayCode.Trim(), StringComparison.OrdinalIgnoreCase)))
        {
            throw new InvalidOperationException("Selected bay does not exist.");
        }
    }

    private void EnsureWarehouseCodeAvailable(string code, int? exceptId = null)
    {
        if (_warehouses.Any(x =>
                !MatchesId(x.Id, exceptId) &&
                x.Code.Equals(code.Trim(), StringComparison.OrdinalIgnoreCase)))
        {
            throw new InvalidOperationException("Warehouse code already exists.");
        }
    }

    private void EnsureBayCodeAvailable(string warehouseCode, string code, int? exceptId = null)
    {
        if (_bays.Any(x =>
                !MatchesId(x.Id, exceptId) &&
                x.WarehouseCode.Equals(warehouseCode.Trim(), StringComparison.OrdinalIgnoreCase) &&
                x.Code.Equals(code.Trim(), StringComparison.OrdinalIgnoreCase)))
        {
            throw new InvalidOperationException("Bay code already exists in the selected warehouse.");
        }
    }

    private void EnsureSubBayCodeAvailable(string bayCode, string code, int? exceptId = null)
    {
        if (_subBays.Any(x =>
                !MatchesId(x.Id, exceptId) &&
                x.BayCode.Equals(bayCode.Trim(), StringComparison.OrdinalIgnoreCase) &&
                x.Code.Equals(code.Trim(), StringComparison.OrdinalIgnoreCase)))
        {
            throw new InvalidOperationException("Sub-bay code already exists in the selected bay.");
        }
    }

    private void EnsureRowCodeAvailable(string code, int? exceptId = null)
    {
        if (_locations.Any(x =>
                !MatchesId(x.Id, exceptId) &&
                x.Code.Equals(code.Trim(), StringComparison.OrdinalIgnoreCase)))
        {
            throw new InvalidOperationException("Row code already exists.");
        }
    }

    private static void EnsureOccupiedWithinCapacity(int occupied, int capacity)
    {
        if (occupied > capacity)
        {
            throw new InvalidOperationException("Occupied quantity cannot be greater than capacity.");
        }
    }

    private static bool MatchesId(int id, int? expectedId) => expectedId.HasValue && id == expectedId.Value;

    private static int NextId(IEnumerable<int> ids) => ids.DefaultIfEmpty(0).Max() + 1;

    private string GetWarehouseName(string warehouseCode)
    {
        return _warehouses.First(x => x.Code.Equals(warehouseCode, StringComparison.OrdinalIgnoreCase)).Name;
    }

    private string GetBayName(string warehouseCode, string bayCode)
    {
        return _bays.First(x =>
            x.WarehouseCode.Equals(warehouseCode, StringComparison.OrdinalIgnoreCase) &&
            x.Code.Equals(bayCode, StringComparison.OrdinalIgnoreCase)).Name;
    }
}
