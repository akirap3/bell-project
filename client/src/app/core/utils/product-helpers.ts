export function getStockClass(stock: number): string {
  if (stock > 20) return 'stock-ok';
  if (stock > 0) return 'stock-low';
  return 'stock-out';
}
