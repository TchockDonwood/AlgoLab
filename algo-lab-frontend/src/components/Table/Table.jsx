export default function Table({
  columns = [],
  data = [],
  rowKey,
  onRowClick,
  emptyMessage = "No data available.",
}) {
  if (!data.length) {
    return (
      <div className="ui-table-empty">
        {emptyMessage}
      </div>
    );
  }

  return (
    <div className="ui-table-wrapper">
      <table className="ui-table">
        <thead>
          <tr>
            {columns.map((column) => (
              <th key={column.key}>
                {column.label}
              </th>
            ))}
          </tr>
        </thead>

        <tbody>
          {data.map((row, index) => {
            const key = rowKey
              ? rowKey(row)
              : row.id ?? index;

            const clickable =
              Boolean(onRowClick);

            return (
              <tr
                key={key}
                className={
                  clickable
                    ? "ui-table-row-clickable"
                    : ""
                }
                onClick={
                  clickable
                    ? () => onRowClick(row)
                    : undefined
                }
              >
                {columns.map((column) => (
                  <td key={column.key}>
                    {column.render
                      ? column.render(row)
                      : row[column.key] ?? "—"}
                  </td>
                ))}
              </tr>
            );
          })}
        </tbody>
      </table>
    </div>
  );
}