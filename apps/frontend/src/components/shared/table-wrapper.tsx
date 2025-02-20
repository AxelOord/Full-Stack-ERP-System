"use client"

import { DataTable } from "@/components/ui/data-table";
import { Link, PaginatedResponse } from "@/services";
import { useState, useCallback, useEffect } from "react";
import { requestFromUrl as __request } from "@/services/core/request";

interface TableWrapperProps<T> {
  fetchData: (
    pageNumber?: number,
    pageSize?: number,
    sortBy?: string,
    searchTerm?: string
  ) => Promise<PaginatedResponse<T>>;
}

export default function TableWrapper<T extends object>({
  fetchData,
  }: TableWrapperProps<T>) {
  const [data, setData] = useState<PaginatedResponse<T> | null>(null);

  const loadData = useCallback(
    async (link?: Link) => { // TODO: make Link type that has rel href and method
      const response = link ? await __request<PaginatedResponse<T>>(link) : await fetchData(undefined, 25);
      setData(response);
    },
    [fetchData]
  );

  useEffect(() => {
    loadData();
  }, [loadData]);

  const field = data?.metadata?.columns?.at(1)?.field; // TODO: define in api

  return (
    <DataTable<T>
      data={data}
      onSearch={field ? (term: string) => fetchData(undefined, 25, field, term).then(setData) : null}
      onPaginate={(link?: Link) => loadData(link)}
    />
  );
}
