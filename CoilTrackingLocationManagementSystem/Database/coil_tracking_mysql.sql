CREATE DATABASE IF NOT EXISTS coil_tracking_db;
USE coil_tracking_db;

CREATE TABLE suppliers (
    supplier_id INT AUTO_INCREMENT PRIMARY KEY,
    supplier_name VARCHAR(150) NOT NULL,
    contact_person VARCHAR(100) NULL,
    phone_no VARCHAR(30) NULL,
    is_active TINYINT(1) NOT NULL DEFAULT 1
);

CREATE TABLE material_grades (
    grade_id INT AUTO_INCREMENT PRIMARY KEY,
    grade_code VARCHAR(30) NOT NULL UNIQUE,
    grade_name VARCHAR(100) NOT NULL
);

CREATE TABLE warehouses (
    warehouse_id INT AUTO_INCREMENT PRIMARY KEY,
    warehouse_code VARCHAR(30) NOT NULL UNIQUE,
    warehouse_name VARCHAR(100) NOT NULL,
    description VARCHAR(255) NULL,
    is_active TINYINT(1) NOT NULL DEFAULT 1,
    created_date DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    modified_date DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE bays (
    bay_id INT AUTO_INCREMENT PRIMARY KEY,
    warehouse_id INT NOT NULL,
    bay_code VARCHAR(30) NOT NULL,
    bay_name VARCHAR(100) NOT NULL,
    is_active TINYINT(1) NOT NULL DEFAULT 1,
    created_date DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    modified_date DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT uq_bay UNIQUE (warehouse_id, bay_code),
    CONSTRAINT fk_bay_warehouse FOREIGN KEY (warehouse_id) REFERENCES warehouses(warehouse_id)
);

CREATE TABLE sub_bays (
    sub_bay_id INT AUTO_INCREMENT PRIMARY KEY,
    bay_id INT NOT NULL,
    sub_bay_code VARCHAR(30) NOT NULL,
    sub_bay_name VARCHAR(100) NOT NULL,
    is_active TINYINT(1) NOT NULL DEFAULT 1,
    created_date DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    modified_date DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT uq_sub_bay UNIQUE (bay_id, sub_bay_code),
    CONSTRAINT fk_sub_bay_bay FOREIGN KEY (bay_id) REFERENCES bays(bay_id)
);

CREATE TABLE storage_rows (
    row_id INT AUTO_INCREMENT PRIMARY KEY,
    sub_bay_id INT NOT NULL,
    row_code VARCHAR(30) NOT NULL,
    row_name VARCHAR(100) NOT NULL,
    row_label VARCHAR(100) NULL,
    capacity INT NOT NULL,
    occupied INT NOT NULL DEFAULT 0,
    is_active TINYINT(1) NOT NULL DEFAULT 1,
    created_date DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    modified_date DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT uq_storage_row UNIQUE (sub_bay_id, row_code),
    CONSTRAINT fk_storage_row_sub_bay FOREIGN KEY (sub_bay_id) REFERENCES sub_bays(sub_bay_id)
);

CREATE TABLE coils (
    coil_id INT AUTO_INCREMENT PRIMARY KEY,
    coil_number VARCHAR(50) NOT NULL UNIQUE,
    heat_number VARCHAR(50) NOT NULL,
    supplier_id INT NULL,
    grade_id INT NULL,
    thickness DECIMAL(10,2) NOT NULL,
    width DECIMAL(10,2) NOT NULL,
    weight DECIMAL(10,2) NOT NULL,
    received_date DATETIME NOT NULL,
    current_row_id INT NULL,
    status VARCHAR(30) NOT NULL,
    remarks VARCHAR(255) NULL,
    CONSTRAINT fk_coils_supplier FOREIGN KEY (supplier_id) REFERENCES suppliers(supplier_id),
    CONSTRAINT fk_coils_grade FOREIGN KEY (grade_id) REFERENCES material_grades(grade_id),
    CONSTRAINT fk_coils_row FOREIGN KEY (current_row_id) REFERENCES storage_rows(row_id)
);

CREATE TABLE inward_entries (
    inward_id INT AUTO_INCREMENT PRIMARY KEY,
    coil_id INT NOT NULL,
    grn_number VARCHAR(50) NOT NULL,
    vehicle_number VARCHAR(30) NULL,
    received_by VARCHAR(80) NOT NULL,
    inward_date DATETIME NOT NULL,
    qc_status VARCHAR(30) NULL,
    CONSTRAINT fk_inward_coil FOREIGN KEY (coil_id) REFERENCES coils(coil_id)
);

CREATE TABLE coil_movements (
    movement_id INT AUTO_INCREMENT PRIMARY KEY,
    coil_id INT NOT NULL,
    from_row_id INT NULL,
    to_row_id INT NULL,
    activity_type VARCHAR(50) NOT NULL,
    reference_number VARCHAR(50) NULL,
    movement_date DATETIME NOT NULL,
    updated_by VARCHAR(80) NOT NULL,
    remarks VARCHAR(255) NULL,
    CONSTRAINT fk_movement_coil FOREIGN KEY (coil_id) REFERENCES coils(coil_id),
    CONSTRAINT fk_movement_from_row FOREIGN KEY (from_row_id) REFERENCES storage_rows(row_id),
    CONSTRAINT fk_movement_to_row FOREIGN KEY (to_row_id) REFERENCES storage_rows(row_id)
);

CREATE TABLE dispatch_entries (
    dispatch_id INT AUTO_INCREMENT PRIMARY KEY,
    coil_id INT NOT NULL,
    dispatch_no VARCHAR(50) NOT NULL,
    customer_name VARCHAR(120) NOT NULL,
    vehicle_number VARCHAR(30) NULL,
    dispatch_date DATETIME NOT NULL,
    dispatched_by VARCHAR(80) NOT NULL,
    CONSTRAINT fk_dispatch_coil FOREIGN KEY (coil_id) REFERENCES coils(coil_id)
);

ALTER TABLE warehouses
    ADD COLUMN IF NOT EXISTS created_date DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    ADD COLUMN IF NOT EXISTS modified_date DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP;

ALTER TABLE bays
    ADD COLUMN IF NOT EXISTS created_date DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    ADD COLUMN IF NOT EXISTS modified_date DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP;

ALTER TABLE sub_bays
    ADD COLUMN IF NOT EXISTS created_date DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    ADD COLUMN IF NOT EXISTS modified_date DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP;

ALTER TABLE storage_rows
    ADD COLUMN IF NOT EXISTS row_label VARCHAR(100) NULL,
    ADD COLUMN IF NOT EXISTS occupied INT NOT NULL DEFAULT 0,
    ADD COLUMN IF NOT EXISTS created_date DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    ADD COLUMN IF NOT EXISTS modified_date DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP;

INSERT INTO suppliers (supplier_name, contact_person, phone_no)
VALUES
('Tata Steel', 'Rahul', '9000000001'),
('JSW', 'Pankaj', '9000000002');

INSERT INTO material_grades (grade_code, grade_name)
VALUES
('CRCA', 'Cold Rolled Close Annealed'),
('HR', 'Hot Rolled'),
('GP', 'Galvanized Plain');

INSERT INTO warehouses (warehouse_code, warehouse_name, description, created_date, modified_date)
VALUES
('WH-01', 'Main Warehouse', 'Primary coil storage warehouse', NOW(), NOW()),
('WH-02', 'Dispatch Warehouse', 'Finished material and dispatch holding area', NOW(), NOW());

INSERT INTO bays (warehouse_id, bay_code, bay_name, created_date, modified_date)
VALUES
(1, 'BAY-A', 'Bay A', NOW(), NOW()),
(1, 'BAY-B', 'Bay B', NOW(), NOW()),
(2, 'BAY-D', 'Dispatch Bay', NOW(), NOW());

INSERT INTO sub_bays (bay_id, sub_bay_code, sub_bay_name, created_date, modified_date)
VALUES
(1, 'SB-01', 'SubBay 01', NOW(), NOW()),
(1, 'SB-02', 'SubBay 02', NOW(), NOW()),
(2, 'SB-01', 'Inspection SubBay', NOW(), NOW()),
(3, 'SB-01', 'Dispatch Staging', NOW(), NOW());

INSERT INTO storage_rows (sub_bay_id, row_code, row_name, row_label, capacity, occupied, created_date, modified_date)
VALUES
(1, 'ROW-A-01', 'Main Receiving Row 01', 'Row 01', 12, 8, NOW(), NOW()),
(2, 'ROW-A-02', 'Main Receiving Row 02', 'Row 02', 12, 10, NOW(), NOW()),
(3, 'ROW-B-01', 'Inspection Row 01', 'Row 01', 8, 3, NOW(), NOW()),
(4, 'ROW-D-01', 'Dispatch Row 01', 'Row 01', 6, 4, NOW(), NOW());

DELIMITER $$

DROP PROCEDURE IF EXISTS Web_get_warehouses $$
CREATE PROCEDURE Web_get_warehouses()
BEGIN
    SELECT warehouse_id, warehouse_code, warehouse_name, description, is_active, created_date, modified_date
    FROM warehouses
    ORDER BY warehouse_name;
END $$

DROP PROCEDURE IF EXISTS Web_get_bays $$
CREATE PROCEDURE Web_get_bays()
BEGIN
    SELECT b.bay_id, w.warehouse_code, b.bay_code, b.bay_name, b.is_active, b.created_date, b.modified_date
    FROM bays b
    INNER JOIN warehouses w ON w.warehouse_id = b.warehouse_id
    ORDER BY w.warehouse_name, b.bay_name;
END $$

DROP PROCEDURE IF EXISTS Web_get_sub_bays $$
CREATE PROCEDURE Web_get_sub_bays()
BEGIN
    SELECT sb.sub_bay_id, w.warehouse_code, b.bay_code, sb.sub_bay_code, sb.sub_bay_name, sb.is_active, sb.created_date, sb.modified_date
    FROM sub_bays sb
    INNER JOIN bays b ON b.bay_id = sb.bay_id
    INNER JOIN warehouses w ON w.warehouse_id = b.warehouse_id
    ORDER BY w.warehouse_name, b.bay_name, sb.sub_bay_name;
END $$

DROP PROCEDURE IF EXISTS Web_get_rows $$
CREATE PROCEDURE Web_get_rows()
BEGIN
    SELECT sr.row_id, sr.row_code, sr.row_name, COALESCE(sr.row_label, sr.row_name) AS row_label,
           sr.capacity, COALESCE(sr.occupied, 0) AS occupied, sr.is_active,
           sr.created_date, sr.modified_date,
           w.warehouse_name, b.bay_name, sb.sub_bay_name
    FROM storage_rows sr
    INNER JOIN sub_bays sb ON sb.sub_bay_id = sr.sub_bay_id
    INNER JOIN bays b ON b.bay_id = sb.bay_id
    INNER JOIN warehouses w ON w.warehouse_id = b.warehouse_id
    ORDER BY w.warehouse_name, b.bay_name, sb.sub_bay_name, sr.row_name;
END $$

DROP PROCEDURE IF EXISTS Web_get_coils $$
CREATE PROCEDURE Web_get_coils()
BEGIN
    SELECT c.coil_id, c.coil_number, c.heat_number, g.grade_code, c.thickness, c.width, c.weight,
           c.received_date, c.status,
           CONCAT(sr.row_code, ' (', w.warehouse_name, ' / ', b.bay_name, ' / ', sb.sub_bay_name, ' / ', COALESCE(sr.row_label, sr.row_name), ')') AS location_path
    FROM coils c
    LEFT JOIN material_grades g ON g.grade_id = c.grade_id
    LEFT JOIN storage_rows sr ON sr.row_id = c.current_row_id
    LEFT JOIN sub_bays sb ON sb.sub_bay_id = sr.sub_bay_id
    LEFT JOIN bays b ON b.bay_id = sb.bay_id
    LEFT JOIN warehouses w ON w.warehouse_id = b.warehouse_id
    ORDER BY c.received_date DESC;
END $$

DROP PROCEDURE IF EXISTS Web_get_coil_movements $$
CREATE PROCEDURE Web_get_coil_movements()
BEGIN
    SELECT cm.movement_id, c.coil_number, cm.activity_type, cm.reference_number, cm.updated_by, cm.movement_date,
           CONCAT(COALESCE(fr.row_code, 'N/A'), CASE WHEN fw.warehouse_name IS NULL THEN '' ELSE CONCAT(' (', fw.warehouse_name, ' / ', fb.bay_name, ' / ', fsb.sub_bay_name, ' / ', COALESCE(fr.row_label, fr.row_name), ')') END) AS from_location,
           CONCAT(COALESCE(tr.row_code, 'N/A'), CASE WHEN tw.warehouse_name IS NULL THEN '' ELSE CONCAT(' (', tw.warehouse_name, ' / ', tb.bay_name, ' / ', tsb.sub_bay_name, ' / ', COALESCE(tr.row_label, tr.row_name), ')') END) AS to_location
    FROM coil_movements cm
    INNER JOIN coils c ON c.coil_id = cm.coil_id
    LEFT JOIN storage_rows fr ON fr.row_id = cm.from_row_id
    LEFT JOIN sub_bays fsb ON fsb.sub_bay_id = fr.sub_bay_id
    LEFT JOIN bays fb ON fb.bay_id = fsb.bay_id
    LEFT JOIN warehouses fw ON fw.warehouse_id = fb.warehouse_id
    LEFT JOIN storage_rows tr ON tr.row_id = cm.to_row_id
    LEFT JOIN sub_bays tsb ON tsb.sub_bay_id = tr.sub_bay_id
    LEFT JOIN bays tb ON tb.bay_id = tsb.bay_id
    LEFT JOIN warehouses tw ON tw.warehouse_id = tb.warehouse_id
    ORDER BY cm.movement_date DESC;
END $$

DROP PROCEDURE IF EXISTS Web_get_grades $$
CREATE PROCEDURE Web_get_grades()
BEGIN
    SELECT grade_code
    FROM material_grades
    ORDER BY grade_code;
END $$

DROP PROCEDURE IF EXISTS Web_get_suppliers $$
CREATE PROCEDURE Web_get_suppliers()
BEGIN
    SELECT supplier_name
    FROM suppliers
    ORDER BY supplier_name;
END $$

DROP PROCEDURE IF EXISTS Web_create_inward_entry $$
CREATE PROCEDURE Web_create_inward_entry(
    IN p_coil_number VARCHAR(50),
    IN p_heat_number VARCHAR(50),
    IN p_grade_code VARCHAR(30),
    IN p_thickness DECIMAL(10,2),
    IN p_width DECIMAL(10,2),
    IN p_weight DECIMAL(10,2),
    IN p_row_id INT,
    IN p_grn_number VARCHAR(50),
    IN p_received_by VARCHAR(80),
    IN p_received_date DATETIME,
    IN p_remarks VARCHAR(255)
)
BEGIN
    DECLARE v_grade_id INT;
    DECLARE v_row_id INT;
    DECLARE v_capacity INT;
    DECLARE v_occupied INT;
    DECLARE v_coil_id INT;

    IF EXISTS (SELECT 1 FROM coils WHERE coil_number = p_coil_number) THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Coil number already exists.';
    END IF;

    SELECT grade_id INTO v_grade_id
    FROM material_grades
    WHERE grade_code = p_grade_code
    LIMIT 1;

    IF v_grade_id IS NULL THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Selected grade does not exist.';
    END IF;

    SELECT row_id, capacity, occupied INTO v_row_id, v_capacity, v_occupied
    FROM storage_rows
    WHERE row_id = p_row_id AND is_active = 1
    LIMIT 1;

    IF v_row_id IS NULL THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Selected row does not exist.';
    END IF;

    IF v_occupied >= v_capacity THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Selected row is already full.';
    END IF;

    START TRANSACTION;

    INSERT INTO coils (coil_number, heat_number, grade_id, thickness, width, weight, received_date, current_row_id, status, remarks)
    VALUES (p_coil_number, p_heat_number, v_grade_id, p_thickness, p_width, p_weight, p_received_date, v_row_id, 'Stored', p_remarks);

    SET v_coil_id = LAST_INSERT_ID();

    INSERT INTO inward_entries (coil_id, grn_number, vehicle_number, received_by, inward_date, qc_status)
    VALUES (v_coil_id, p_grn_number, NULL, p_received_by, p_received_date, 'Pending');

    INSERT INTO coil_movements (coil_id, from_row_id, to_row_id, activity_type, reference_number, movement_date, updated_by, remarks)
    VALUES (v_coil_id, NULL, v_row_id, 'Inward Entry', p_grn_number, p_received_date, p_received_by, p_remarks);

    UPDATE storage_rows
    SET occupied = occupied + 1,
        modified_date = NOW()
    WHERE row_id = v_row_id;

    COMMIT;
END $$

DROP PROCEDURE IF EXISTS Web_create_warehouse $$
CREATE PROCEDURE Web_create_warehouse(
    IN p_code VARCHAR(30),
    IN p_name VARCHAR(100),
    IN p_description VARCHAR(255),
    IN p_is_active TINYINT
)
BEGIN
    IF EXISTS (SELECT 1 FROM warehouses WHERE warehouse_code = p_code) THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Warehouse code already exists.';
    END IF;

    INSERT INTO warehouses (warehouse_code, warehouse_name, description, is_active, created_date, modified_date)
    VALUES (p_code, p_name, p_description, p_is_active, NOW(), NOW());
END $$

DROP PROCEDURE IF EXISTS Web_update_warehouse $$
CREATE PROCEDURE Web_update_warehouse(
    IN p_id INT,
    IN p_code VARCHAR(30),
    IN p_name VARCHAR(100),
    IN p_description VARCHAR(255),
    IN p_is_active TINYINT
)
BEGIN
    IF EXISTS (SELECT 1 FROM warehouses WHERE warehouse_code = p_code AND warehouse_id <> p_id) THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Warehouse code already exists.';
    END IF;

    UPDATE warehouses
    SET warehouse_code = p_code,
        warehouse_name = p_name,
        description = p_description,
        is_active = p_is_active,
        modified_date = NOW()
    WHERE warehouse_id = p_id;
END $$

DROP PROCEDURE IF EXISTS Web_delete_warehouse $$
CREATE PROCEDURE Web_delete_warehouse(IN p_id INT)
BEGIN
    IF EXISTS (SELECT 1 FROM bays WHERE warehouse_id = p_id) THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Delete bays under this warehouse first.';
    END IF;

    DELETE FROM warehouses WHERE warehouse_id = p_id;
END $$

DROP PROCEDURE IF EXISTS Web_create_bay $$
CREATE PROCEDURE Web_create_bay(
    IN p_warehouse_code VARCHAR(30),
    IN p_code VARCHAR(30),
    IN p_name VARCHAR(100),
    IN p_is_active TINYINT
)
BEGIN
    DECLARE v_warehouse_id INT;

    SELECT warehouse_id INTO v_warehouse_id
    FROM warehouses
    WHERE warehouse_code = p_warehouse_code
    LIMIT 1;

    IF v_warehouse_id IS NULL THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Selected warehouse does not exist.';
    END IF;

    IF EXISTS (SELECT 1 FROM bays WHERE warehouse_id = v_warehouse_id AND bay_code = p_code) THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Bay code already exists in the selected warehouse.';
    END IF;

    INSERT INTO bays (warehouse_id, bay_code, bay_name, is_active, created_date, modified_date)
    VALUES (v_warehouse_id, p_code, p_name, p_is_active, NOW(), NOW());
END $$

DROP PROCEDURE IF EXISTS Web_update_bay $$
CREATE PROCEDURE Web_update_bay(
    IN p_id INT,
    IN p_warehouse_code VARCHAR(30),
    IN p_code VARCHAR(30),
    IN p_name VARCHAR(100),
    IN p_is_active TINYINT
)
BEGIN
    DECLARE v_warehouse_id INT;

    SELECT warehouse_id INTO v_warehouse_id
    FROM warehouses
    WHERE warehouse_code = p_warehouse_code
    LIMIT 1;

    IF v_warehouse_id IS NULL THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Selected warehouse does not exist.';
    END IF;

    IF EXISTS (SELECT 1 FROM bays WHERE warehouse_id = v_warehouse_id AND bay_code = p_code AND bay_id <> p_id) THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Bay code already exists in the selected warehouse.';
    END IF;

    UPDATE bays
    SET warehouse_id = v_warehouse_id,
        bay_code = p_code,
        bay_name = p_name,
        is_active = p_is_active,
        modified_date = NOW()
    WHERE bay_id = p_id;
END $$

DROP PROCEDURE IF EXISTS Web_delete_bay $$
CREATE PROCEDURE Web_delete_bay(IN p_id INT)
BEGIN
    IF EXISTS (SELECT 1 FROM sub_bays WHERE bay_id = p_id) THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Delete sub-bays under this bay first.';
    END IF;

    DELETE FROM bays WHERE bay_id = p_id;
END $$

DROP PROCEDURE IF EXISTS Web_create_sub_bay $$
CREATE PROCEDURE Web_create_sub_bay(
    IN p_warehouse_code VARCHAR(30),
    IN p_bay_code VARCHAR(30),
    IN p_code VARCHAR(30),
    IN p_name VARCHAR(100),
    IN p_is_active TINYINT
)
BEGIN
    DECLARE v_bay_id INT;

    SELECT b.bay_id INTO v_bay_id
    FROM bays b
    INNER JOIN warehouses w ON w.warehouse_id = b.warehouse_id
    WHERE w.warehouse_code = p_warehouse_code AND b.bay_code = p_bay_code
    LIMIT 1;

    IF v_bay_id IS NULL THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Selected bay does not exist.';
    END IF;

    IF EXISTS (SELECT 1 FROM sub_bays WHERE bay_id = v_bay_id AND sub_bay_code = p_code) THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Sub-bay code already exists in the selected bay.';
    END IF;

    INSERT INTO sub_bays (bay_id, sub_bay_code, sub_bay_name, is_active, created_date, modified_date)
    VALUES (v_bay_id, p_code, p_name, p_is_active, NOW(), NOW());
END $$

DROP PROCEDURE IF EXISTS Web_update_sub_bay $$
CREATE PROCEDURE Web_update_sub_bay(
    IN p_id INT,
    IN p_warehouse_code VARCHAR(30),
    IN p_bay_code VARCHAR(30),
    IN p_code VARCHAR(30),
    IN p_name VARCHAR(100),
    IN p_is_active TINYINT
)
BEGIN
    DECLARE v_bay_id INT;

    SELECT b.bay_id INTO v_bay_id
    FROM bays b
    INNER JOIN warehouses w ON w.warehouse_id = b.warehouse_id
    WHERE w.warehouse_code = p_warehouse_code AND b.bay_code = p_bay_code
    LIMIT 1;

    IF v_bay_id IS NULL THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Selected bay does not exist.';
    END IF;

    IF EXISTS (SELECT 1 FROM sub_bays WHERE bay_id = v_bay_id AND sub_bay_code = p_code AND sub_bay_id <> p_id) THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Sub-bay code already exists in the selected bay.';
    END IF;

    UPDATE sub_bays
    SET bay_id = v_bay_id,
        sub_bay_code = p_code,
        sub_bay_name = p_name,
        is_active = p_is_active,
        modified_date = NOW()
    WHERE sub_bay_id = p_id;
END $$

DROP PROCEDURE IF EXISTS Web_delete_sub_bay $$
CREATE PROCEDURE Web_delete_sub_bay(IN p_id INT)
BEGIN
    IF EXISTS (SELECT 1 FROM storage_rows WHERE sub_bay_id = p_id) THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Delete rows under this sub-bay first.';
    END IF;

    DELETE FROM sub_bays WHERE sub_bay_id = p_id;
END $$

DROP PROCEDURE IF EXISTS Web_create_row $$
CREATE PROCEDURE Web_create_row(
    IN p_sub_bay_id INT,
    IN p_code VARCHAR(30),
    IN p_name VARCHAR(100),
    IN p_row_label VARCHAR(100),
    IN p_capacity INT,
    IN p_occupied INT,
    IN p_is_active TINYINT
)
BEGIN
    IF p_occupied > p_capacity THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Occupied quantity cannot be greater than capacity.';
    END IF;

    IF EXISTS (SELECT 1 FROM storage_rows WHERE row_code = p_code) THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Row code already exists.';
    END IF;

    INSERT INTO storage_rows (sub_bay_id, row_code, row_name, row_label, capacity, occupied, is_active, created_date, modified_date)
    VALUES (p_sub_bay_id, p_code, p_name, p_row_label, p_capacity, p_occupied, p_is_active, NOW(), NOW());
END $$

DROP PROCEDURE IF EXISTS Web_update_row $$
CREATE PROCEDURE Web_update_row(
    IN p_id INT,
    IN p_sub_bay_id INT,
    IN p_code VARCHAR(30),
    IN p_name VARCHAR(100),
    IN p_row_label VARCHAR(100),
    IN p_capacity INT,
    IN p_occupied INT,
    IN p_is_active TINYINT
)
BEGIN
    DECLARE v_old_code VARCHAR(30);

    IF p_occupied > p_capacity THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Occupied quantity cannot be greater than capacity.';
    END IF;

    IF EXISTS (SELECT 1 FROM storage_rows WHERE row_code = p_code AND row_id <> p_id) THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Row code already exists.';
    END IF;

    SELECT row_code INTO v_old_code
    FROM storage_rows
    WHERE row_id = p_id
    LIMIT 1;

    UPDATE storage_rows
    SET sub_bay_id = p_sub_bay_id,
        row_code = p_code,
        row_name = p_name,
        row_label = p_row_label,
        capacity = p_capacity,
        occupied = p_occupied,
        is_active = p_is_active,
        modified_date = NOW()
    WHERE row_id = p_id;

    UPDATE coils
    SET current_row_id = p_id
    WHERE current_row_id = p_id;
END $$

DROP PROCEDURE IF EXISTS Web_delete_row $$
CREATE PROCEDURE Web_delete_row(IN p_id INT)
BEGIN
    IF EXISTS (SELECT 1 FROM coils WHERE current_row_id = p_id) THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'This row is assigned to one or more coils.';
    END IF;

    DELETE FROM storage_rows WHERE row_id = p_id;
END $$

DELIMITER ;
