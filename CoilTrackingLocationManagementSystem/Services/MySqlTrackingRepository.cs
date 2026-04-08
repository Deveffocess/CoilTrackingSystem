using CoilTrackingLocationManagementSystem.Models;
using MySqlConnector;
using System.Data;

namespace CoilTrackingLocationManagementSystem.Services;

public class MySqlTrackingRepository : ITrackingRepository
{
    private readonly string _connectionString;
    private const string CreateWarehouseProcedure = "Web_create_warehouse";
    private const string UpdateWarehouseProcedure = "Web_update_warehouse";
    private const string DeleteWarehouseProcedure = "Web_delete_warehouse";
    private const string CreateBayProcedure = "Web_create_bay";
    private const string UpdateBayProcedure = "Web_update_bay";
    private const string DeleteBayProcedure = "Web_delete_bay";
    private const string CreateSubBayProcedure = "Web_create_sub_bay";
    private const string UpdateSubBayProcedure = "Web_update_sub_bay";
    private const string DeleteSubBayProcedure = "Web_delete_sub_bay";
    private const string CreateRowProcedure = "Web_create_row";
    private const string UpdateRowProcedure = "Web_update_row";
    private const string DeleteRowProcedure = "Web_delete_row";
    private const string GetWarehousesProcedure = "Web_Get_Warehouses";
    private const string GetWarehousesProcedureAlt = "Web_get_warehouses";
    private const string GetBaysProcedure = "Web_Get_Bays";
    private const string GetBaysProcedureAlt = "Web_get_bays";
    private const string GetSubBaysProcedure = "Web_Get_SubBays";
    private const string GetSubBaysProcedureAlt = "Web_get_sub_bays";
    private const string GetRowsProcedure = "Web_Get_Rows";
    private const string GetRowsProcedureAlt = "Web_get_rows";
    private const string GetCoilsProcedure = "Web_get_coils";
    private const string GetMovementsProcedure = "Web_Get_Coil_Movements";
    private const string GetMovementsProcedureAlt = "Web_get_coil_movements";
    private const string GetGradesProcedure = "Web_Get_Grades";
    private const string GetGradesProcedureAlt = "Web_get_grades";
    private const string GetSuppliersProcedure = "Web_Get_suppliers";
    private const string GetSuppliersProcedureAlt = "Web_get_suppliers";
    private const string CreateInwardEntryProcedure = "Web_create_inward_entry";

    public MySqlTrackingRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public DashboardViewModel GetDashboard()
    {
        var coils = GetCoilsInternal();
        var locations = GetRows();
        var movements = GetMovementsInternal();

        return new DashboardViewModel
        {
            TotalCoils = coils.Count,
            StoredCoils = coils.Count(x => x.Status == CoilStatus.Stored),
            InspectionPending = coils.Count(x => x.Status == CoilStatus.InInspection),
            DispatchReady = coils.Count(x => x.Status == CoilStatus.Issued),
            RecentCoils = coils.OrderByDescending(x => x.ReceivedDate).Take(10).ToList(),
            Locations = locations,
            RecentMovements = movements.OrderByDescending(x => x.MovementDate).Take(10).ToList()
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
            InwardCoils = GetCoilsInternal().OrderByDescending(x => x.ReceivedDate).ToList(),
            Movements = GetMovementsInternal().OrderByDescending(x => x.MovementDate).ToList(),
            Locations = GetRows(),
            Grades = GetGrades()
        };
    }

    public ReportsViewModel GetReports()
    {
        return new ReportsViewModel
        {
            Coils = GetCoilsInternal().OrderBy(x => x.CoilNumber).ToList(),
            Movements = GetMovementsInternal().OrderByDescending(x => x.MovementDate).ToList()
        };
    }

    public void CreateInwardEntry(InwardEntryInputModel input)
    {
        ExecuteProcedure(CreateInwardEntryProcedure, cmd =>
        {
            cmd.Parameters.AddWithValue("p_coil_number", input.CoilNumber.Trim());
            cmd.Parameters.AddWithValue("p_heat_number", input.HeatNumber.Trim());
            cmd.Parameters.AddWithValue("p_grade_code", input.Grade.Trim());
            cmd.Parameters.AddWithValue("p_thickness", input.Thickness);
            cmd.Parameters.AddWithValue("p_width", input.Width);
            cmd.Parameters.AddWithValue("p_weight", input.Weight);
            cmd.Parameters.AddWithValue("p_row_id", input.LocationId);
            cmd.Parameters.AddWithValue("p_grn_number", input.GrnNumber.Trim());
            cmd.Parameters.AddWithValue("p_received_by", input.ReceivedBy.Trim());
            cmd.Parameters.AddWithValue("p_received_date", input.ReceivedDate == default ? DateTime.Now : input.ReceivedDate);
            cmd.Parameters.AddWithValue("p_remarks", input.Remarks.Trim());
        });
    }

    public void CreateWarehouse(WarehouseInputModel input)
    {
        ExecuteProcedure(CreateWarehouseProcedure, cmd =>
        {
            cmd.Parameters.AddWithValue("p_code", input.Code.Trim());
            cmd.Parameters.AddWithValue("p_name", input.Name.Trim());
            cmd.Parameters.AddWithValue("p_description", input.Description.Trim());
            cmd.Parameters.AddWithValue("p_is_active", input.IsActive);
        });
    }

    public void UpdateWarehouse(WarehouseInputModel input)
    {
        ExecuteProcedure(UpdateWarehouseProcedure, cmd =>
        {
            cmd.Parameters.AddWithValue("p_id", input.Id);
            cmd.Parameters.AddWithValue("p_code", input.Code.Trim());
            cmd.Parameters.AddWithValue("p_name", input.Name.Trim());
            cmd.Parameters.AddWithValue("p_description", input.Description.Trim());
            cmd.Parameters.AddWithValue("p_is_active", input.IsActive);
        });
    }

    public void DeleteWarehouse(int id)
    {
        ExecuteProcedure(DeleteWarehouseProcedure, cmd => cmd.Parameters.AddWithValue("p_id", id));
    }

    public void CreateBay(BayInputModel input)
    {
        ExecuteProcedure(CreateBayProcedure, cmd =>
        {
            cmd.Parameters.AddWithValue("p_warehouse_code", input.WarehouseCode.Trim());
            cmd.Parameters.AddWithValue("p_code", input.Code.Trim());
            cmd.Parameters.AddWithValue("p_name", input.Name.Trim());
            cmd.Parameters.AddWithValue("p_is_active", input.IsActive);
        });
    }

    public void UpdateBay(BayInputModel input)
    {
        ExecuteProcedure(UpdateBayProcedure, cmd =>
        {
            cmd.Parameters.AddWithValue("p_id", input.Id);
            cmd.Parameters.AddWithValue("p_warehouse_code", input.WarehouseCode.Trim());
            cmd.Parameters.AddWithValue("p_code", input.Code.Trim());
            cmd.Parameters.AddWithValue("p_name", input.Name.Trim());
            cmd.Parameters.AddWithValue("p_is_active", input.IsActive);
        });
    }

    public void DeleteBay(int id)
    {
        ExecuteProcedure(DeleteBayProcedure, cmd => cmd.Parameters.AddWithValue("p_id", id));
    }

    public void CreateSubBay(SubBayInputModel input)
    {
        ExecuteProcedure(CreateSubBayProcedure, cmd =>
        {
            cmd.Parameters.AddWithValue("p_warehouse_code", input.WarehouseCode.Trim());
            cmd.Parameters.AddWithValue("p_bay_code", input.BayCode.Trim());
            cmd.Parameters.AddWithValue("p_code", input.Code.Trim());
            cmd.Parameters.AddWithValue("p_name", input.Name.Trim());
            cmd.Parameters.AddWithValue("p_is_active", input.IsActive);
        });
    }

    public void UpdateSubBay(SubBayInputModel input)
    {
        ExecuteProcedure(UpdateSubBayProcedure, cmd =>
        {
            cmd.Parameters.AddWithValue("p_id", input.Id);
            cmd.Parameters.AddWithValue("p_warehouse_code", input.WarehouseCode.Trim());
            cmd.Parameters.AddWithValue("p_bay_code", input.BayCode.Trim());
            cmd.Parameters.AddWithValue("p_code", input.Code.Trim());
            cmd.Parameters.AddWithValue("p_name", input.Name.Trim());
            cmd.Parameters.AddWithValue("p_is_active", input.IsActive);
        });
    }

    public void DeleteSubBay(int id)
    {
        ExecuteProcedure(DeleteSubBayProcedure, cmd => cmd.Parameters.AddWithValue("p_id", id));
    }

    public void CreateRow(RowInputModel input)
    {
        if (input.Occupied > input.Capacity)
        {
            throw new InvalidOperationException("Occupied quantity cannot be greater than capacity.");
        }

        ExecuteProcedure(CreateRowProcedure, cmd =>
        {
            cmd.Parameters.AddWithValue("p_sub_bay_id", input.SubBayId);
            cmd.Parameters.AddWithValue("p_code", input.Code.Trim());
            cmd.Parameters.AddWithValue("p_name", input.Name.Trim());
            cmd.Parameters.AddWithValue("p_row_label", input.Row.Trim());
            cmd.Parameters.AddWithValue("p_capacity", input.Capacity);
            cmd.Parameters.AddWithValue("p_occupied", input.Occupied);
            cmd.Parameters.AddWithValue("p_is_active", input.IsActive);
        });
    }

    public void UpdateRow(RowInputModel input)
    {
        if (input.Occupied > input.Capacity)
        {
            throw new InvalidOperationException("Occupied quantity cannot be greater than capacity.");
        }

        ExecuteProcedure(UpdateRowProcedure, cmd =>
        {
            cmd.Parameters.AddWithValue("p_id", input.Id);
            cmd.Parameters.AddWithValue("p_sub_bay_id", input.SubBayId);
            cmd.Parameters.AddWithValue("p_code", input.Code.Trim());
            cmd.Parameters.AddWithValue("p_name", input.Name.Trim());
            cmd.Parameters.AddWithValue("p_row_label", input.Row.Trim());
            cmd.Parameters.AddWithValue("p_capacity", input.Capacity);
            cmd.Parameters.AddWithValue("p_occupied", input.Occupied);
            cmd.Parameters.AddWithValue("p_is_active", input.IsActive);
        });
    }

    public void DeleteRow(int id)
    {
        ExecuteProcedure(DeleteRowProcedure, cmd => cmd.Parameters.AddWithValue("p_id", id));
    }

    public IReadOnlyList<Warehouse> GetWarehouses()
    {
        return QueryProcedure(GetWarehousesProcedure, reader => new Warehouse
        {
            Id = GetInt32(reader, "warehouse_id"),
            Code = GetString(reader, "warehouse_code"),
            Name = GetString(reader, "warehouse_name"),
            Description = IsDbNull(reader, "description") ? string.Empty : GetString(reader, "description"),
            IsActive = GetBoolean(reader, "is_active"),
            CreatedDate = ReadDateTime(reader, "created_date"),
            ModifiedDate = ReadDateTime(reader, "modified_date")
        }, GetWarehousesProcedureAlt);
    }

    public IReadOnlyList<Bay> GetBays()
    {
        return QueryProcedure(GetBaysProcedure, reader => new Bay
        {
            Id = GetInt32(reader, "bay_id"),
            WarehouseCode = GetString(reader, "warehouse_code"),
            Code = GetString(reader, "bay_code"),
            Name = GetString(reader, "bay_name"),
            IsActive = GetBoolean(reader, "is_active"),
            CreatedDate = ReadDateTime(reader, "created_date"),
            ModifiedDate = ReadDateTime(reader, "modified_date")
        }, GetBaysProcedureAlt);
    }

    public IReadOnlyList<SubBay> GetSubBays()
    {
        return QueryProcedure(GetSubBaysProcedure, reader => new SubBay
        {
            Id = GetInt32(reader, "sub_bay_id"),
            WarehouseCode = GetString(reader, "warehouse_code"),
            BayCode = GetString(reader, "bay_code"),
            Code = GetString(reader, "sub_bay_code"),
            Name = GetString(reader, "sub_bay_name"),
            IsActive = GetBoolean(reader, "is_active"),
            CreatedDate = ReadDateTime(reader, "created_date"),
            ModifiedDate = ReadDateTime(reader, "modified_date")
        }, GetSubBaysProcedureAlt);
    }

    public IReadOnlyList<StorageLocation> GetRows()
    {
        return QueryProcedure(GetRowsProcedure, reader => new StorageLocation
        {
            Id = GetInt32(reader, "row_id"),
            Code = GetString(reader, "row_code"),
            Name = GetString(reader, "row_name"),
            Warehouse = GetString(reader, "warehouse_name"),
            Bay = GetString(reader, "bay_name"),
            SubBay = GetString(reader, "sub_bay_name"),
            Row = GetString(reader, "row_label"),
            Capacity = GetInt32(reader, "capacity"),
            Occupied = GetInt32(reader, "occupied"),
            IsActive = GetBoolean(reader, "is_active"),
            CreatedDate = ReadDateTime(reader, "created_date"),
            ModifiedDate = ReadDateTime(reader, "modified_date")
        }, GetRowsProcedureAlt);
    }

    public IReadOnlyList<string> GetGrades()
    {
        return QueryProcedure(GetGradesProcedure, reader => GetString(reader, "grade_code"), GetGradesProcedureAlt);
    }

    public IReadOnlyList<string> GetSuppliers()
    {
        return QueryProcedure(GetSuppliersProcedure, reader => GetString(reader, "supplier_name"), GetSuppliersProcedureAlt);
    }

    private List<Coil> GetCoilsInternal()
    {
        return QueryProcedure(GetCoilsProcedure, reader => new Coil
        {
            Id = GetInt32(reader, "coil_id"),
            CoilNumber = GetString(reader, "coil_number"),
            HeatNumber = GetString(reader, "heat_number"),
            Grade = IsDbNull(reader, "grade_code") ? string.Empty : GetString(reader, "grade_code"),
            Thickness = GetDecimal(reader, "thickness"),
            Width = GetDecimal(reader, "width"),
            Weight = GetDecimal(reader, "weight"),
            ReceivedDate = ReadDateTime(reader, "received_date"),
            Status = ParseStatus(GetString(reader, "status")),
            CurrentLocationCode = IsDbNull(reader, "location_path") ? string.Empty : GetString(reader, "location_path")
        });
    }

    private List<CoilMovement> GetMovementsInternal()
    {
        return QueryProcedure(GetMovementsProcedure, reader => new CoilMovement
        {
            Id = GetInt32(reader, "movement_id"),
            CoilNumber = GetString(reader, "coil_number"),
            Activity = GetString(reader, "activity_type"),
            ReferenceNumber = IsDbNull(reader, "reference_number") ? string.Empty : GetString(reader, "reference_number"),
            UpdatedBy = GetString(reader, "updated_by"),
            MovementDate = ReadDateTime(reader, "movement_date"),
            FromLocation = IsDbNull(reader, "from_location") ? string.Empty : GetString(reader, "from_location"),
            ToLocation = IsDbNull(reader, "to_location") ? string.Empty : GetString(reader, "to_location")
        }, GetMovementsProcedureAlt);
    }

    private List<T> QueryProcedure<T>(string procedureName, Func<MySqlDataReader, T> map, string? alternateProcedureName = null)
    {
        try
        {
            return ExecuteQueryProcedure(procedureName, map);
        }
        catch (MySqlException ex)
        {
            if (ShouldTryAlternateProcedure(ex, alternateProcedureName))
            {
                try
                {
                    return ExecuteQueryProcedure(alternateProcedureName!, map);
                }
                catch (MySqlException alternateEx)
                {
                    throw BuildDatabaseException(alternateProcedureName!, alternateEx);
                }
            }

            throw BuildDatabaseException(procedureName, ex);
        }
    }

    private void ExecuteProcedure(string procedureName, Action<MySqlCommand> parameterize)
    {
        try
        {
            ExecuteNonQueryProcedure(procedureName, parameterize);
        }
        catch (MySqlException ex)
        {
            throw BuildDatabaseException(procedureName, ex);
        }
    }


    private List<T> ExecuteQueryProcedure<T>(string procedureName, Func<MySqlDataReader, T> map)
    {
        using var connection = CreateConnection();
        connection.Open();

        using var command = new MySqlCommand(procedureName, connection)
        {
            CommandType = CommandType.StoredProcedure
        };
        using var reader = command.ExecuteReader();
        var results = new List<T>();

        while (reader.Read())
        {
            results.Add(map(reader));
        }

        return results;
    }

    private void ExecuteNonQueryProcedure(string procedureName, Action<MySqlCommand> parameterize)
    {
        using var connection = CreateConnection();
        connection.Open();

        using var command = new MySqlCommand(procedureName, connection)
        {
            CommandType = CommandType.StoredProcedure
        };
        parameterize(command);
        command.ExecuteNonQuery();
    }


    private MySqlConnection CreateConnection() => new(_connectionString);

    private static bool ShouldTryAlternateProcedure(MySqlException ex, string? alternateProcedureName)
    {
        return !string.IsNullOrWhiteSpace(alternateProcedureName)
            && ex.Message.Contains("does not exist", StringComparison.OrdinalIgnoreCase);
    }

    private static CoilStatus ParseStatus(string status)
    {
        return Enum.TryParse<CoilStatus>(status, true, out var parsed)
            ? parsed
            : CoilStatus.Stored;
    }

    private static DateTime ReadDateTime(MySqlDataReader reader, string columnName)
    {
        return IsDbNull(reader, columnName) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal(columnName));
    }

    private static bool IsDbNull(MySqlDataReader reader, string columnName)
    {
        return reader.IsDBNull(reader.GetOrdinal(columnName));
    }

    private static string GetString(MySqlDataReader reader, string columnName)
    {
        return reader.GetString(reader.GetOrdinal(columnName));
    }

    private static int GetInt32(MySqlDataReader reader, string columnName)
    {
        return reader.GetInt32(reader.GetOrdinal(columnName));
    }

    private static decimal GetDecimal(MySqlDataReader reader, string columnName)
    {
        return reader.GetDecimal(reader.GetOrdinal(columnName));
    }

    private static bool GetBoolean(MySqlDataReader reader, string columnName)
    {
        return reader.GetBoolean(reader.GetOrdinal(columnName));
    }

    private static InvalidOperationException BuildDatabaseException(string procedureName, MySqlException ex)
    {
        var message = $"Database error in {procedureName}: {ex.Message}";
        return new InvalidOperationException(message, ex);
    }
}
