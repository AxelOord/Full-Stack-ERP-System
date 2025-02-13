"use client"

import { DataTable } from "@/components/ui/data-table";
import { PaginatedResponse } from "@/services";
import { useState, useCallback, useEffect } from "react";
import { requestFromUrl as __request } from "@/services/core/request";

interface TableWrapperProps<T> {
  fetchData: (
    pageNumber?: number,
    pageSize?: number,
    sortBy?: string,
    searchTerm?: string
  ) => Promise<PaginatedResponse<T>>;
  searchByProperty: string;
}

export default function TableWrapper<T extends object>({
  fetchData,
  searchByProperty
}: TableWrapperProps<T>) {
  const [data, setData] = useState<PaginatedResponse<T> | null>(null);

  const loadData = useCallback(
    async (link?: string | null | undefined) => {
      const response = link ? await __request<PaginatedResponse<T>>(link) : await fetchData(undefined, 25);
      setData(response);
    },
    [fetchData]
  );

  useEffect(() => {
    loadData();
  }, [loadData]);

  return (
    <DataTable<T>
      data={data}
      onSearch={(term : string) => {
        fetchData(undefined, 25, searchByProperty, term).then(setData);
      }}
      onPaginate={(link?: string | null | undefined) => loadData(link)}
    />
  );
}
