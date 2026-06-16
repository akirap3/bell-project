import os
import pandas as pd
import pymssql

def insert_address(cursor, csv_path='Address.csv'):
	print('importing Address.csv...')
	df_address = pd.read_csv(csv_path)
	for _, row in df_address.iterrows():
		address_id = int(row['AddressID']) if pd.notnull(row.get('AddressID')) else None
		line1 = str(row['AddressLine1']) if pd.notnull(row.get('AddressLine1')) else None
		city = str(row['City']) if pd.notnull(row.get('City')) else None

		cursor.execute(
			"INSERT INTO Address (AddressID, AddressLine1, City) VALUES (%s, %s, %s)",
			(address_id, line1, city),
		)


def insert_salesorderheader(cursor, csv_path='SalesOrderHeader.csv'):
	print('importing SalesOrderHeader.csv...')
	df_sales = pd.read_csv(csv_path)
	df_sales = df_sales.where(pd.notnull(df_sales), None)

	for _, row in df_sales.iterrows():
		sales_id = int(row['SalesOrderID']) if pd.notnull(row.get('SalesOrderID')) else None
		order_date = str(row['OrderDate']) if pd.notnull(row.get('OrderDate')) else None
		actual_ship = row['ActualShipDate'] if pd.notnull(row.get('ActualShipDate')) else None
		customer_id = int(row['CustomerID']) if pd.notnull(row.get('CustomerID')) else None
		shipto_id = int(row['ShipToAddressID']) if pd.notnull(row.get('ShipToAddressID')) else None
		total_due = float(row['TotalDue']) if pd.notnull(row.get('TotalDue')) else None

		cursor.execute(
			(
				"INSERT INTO SalesOrderHeader"
				" (SalesOrderID, OrderDate, ActualShipDate, CustomerID, ShipToAddressID, TotalDue)"
				" VALUES (%s, %s, %s, %s, %s, %s)"
			),
			(sales_id, order_date, actual_ship, customer_id, shipto_id, total_due),
		)


def load_dotenv():
	for path in ['.env', '../.env']:
		if os.path.exists(path):
			with open(path, 'r', encoding='utf-8') as f:
				for line in f:
					line = line.strip()
					if not line or line.startswith('#'):
						continue
					if '=' in line:
						key, val = line.split('=', 1)
						key = key.strip()
						val = val.strip().strip("'\"")
						if key not in os.environ:
							os.environ[key] = val
			break


def main():
	# Load environment variables from .env file if available
	load_dotenv()

	# Read DB connection from environment variables for security
	db_server = os.environ.get('DB_SERVER', 'localhost')
	db_port = int(os.environ.get('DB_PORT', 1433))
	db_user = os.environ.get('DB_USER', 'sa')
	db_password = os.environ.get('MSSQL_SA_PASSWORD', os.environ.get('DB_PASSWORD', ''))
	db_name = os.environ.get('DB_NAME', 'BellBI')

	conn = pymssql.connect(
		server=db_server, port=db_port, user=db_user, password=db_password, database=db_name
	)
	try:
		cursor = conn.cursor()

		insert_address(cursor, csv_path='Address.csv')
		conn.commit()
		print('Address imported successfully!')

		insert_salesorderheader(cursor, csv_path='SalesOrderHeader.csv')
		conn.commit()
		print('SalesOrderHeader imported successfully!')
	finally:
		conn.close()


if __name__ == '__main__':
	main()