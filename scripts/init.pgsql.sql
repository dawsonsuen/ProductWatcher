CREATE TABLE data(
	id bigserial PRIMARY KEY,
	product_id uuid NOT NULL,
	product_model text NULL,
	raw_data text NOT NULL,
	_when timestamp NOT NULL
);

create table price(
    id bigserial primary key,
    data_id bigint not null,
    product_id uuid not null,
    original_price numeric(12,2) not null,
    on_sale_price numeric(12,2),
    company varchar(200) not null,
    description text not null,
    _when timestamp not null,
    additional_data text
);

CREATE TABLE product(
	_id bigserial primary key,
	id uuid NOT NULL,
	code varchar(120) NOT NULL,
	name varchar(255) NOT NULL,
	company varchar(120) NOT NULL
);
