export type {
  TableColumnFilterPlacement,
  TableColumnSortOrders,
  TableColumnSortable,
  TableColumnRenderer,
  TableColumnScope,
  TableColumnFixed,
  TableColumnType,
  PaginationProps,
  XTableProps,
  LoadingConfig,
  TableColumns,
  TableColumn,
  TableProps,
  Layout,
  Effect,
  Align,
  Size
} from "./types";

import type { App } from "vue";
import Table from "./components/table";

export const XTable = Object.assign(Table, {
  install: function (app: App) {
    app.component(Table.name, Table);
  }
});

export default XTable;
