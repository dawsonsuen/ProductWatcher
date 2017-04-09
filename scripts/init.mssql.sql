CREATE TABLE data(
	id int IDENTITY(1,1) NOT NULL PRIMARY KEY,
	product_id uniqueidentifier NOT NULL,
	product_model text NULL,
	raw_data text NOT NULL,
	_when datetime NOT NULL
)
GO
CREATE TABLE price(
	id int IDENTITY(1,1) NOT NULL PRIMARY KEY,
	data_id int NOT NULL,
	product_id uniqueidentifier NOT NULL,
	original_price decimal(12, 2) NOT NULL,
	on_sale_price decimal(12, 2) NULL,
	company nvarchar(120) NOT NULL,
	description text NOT NULL,
	_when datetime NOT NULL,
	additional_data text NULL
)
GO
CREATE TABLE product(
	_id int IDENTITY(1,1) NOT NULL PRIMARY KEY,
	id uniqueidentifier NOT NULL,
	code nvarchar(120) NOT NULL,
	name nvarchar(255) NOT NULL,
	company nvarchar(120) NOT NULL
)
GO