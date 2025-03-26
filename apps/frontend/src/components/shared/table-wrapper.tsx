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
  pageSize?: number;
}

export default function TableWrapper<T extends object>({
  fetchData,
  pageSize = 25,
}: TableWrapperProps<T>) {
  const [data, setData] = useState<PaginatedResponse<T> | null>(null);
  const [activeFilter, setActiveFilter] = useState<{ column: string; value: string } | null>(null);

  // Handler for filter changes
  const handleFilter = useCallback((column: string, value: string) => {
    setActiveFilter({ column, value });
    return fetchData(undefined, pageSize, column, value).then(setData);
  }, [fetchData, pageSize]);

  // Handler for search operations
  const handleSearch = useCallback((searchTerm: string) => {
    const field = data?.metadata?.columns?.at(1)?.field;
    if (field) {
      return handleFilter(field, searchTerm);
    }
    return Promise.resolve();
  }, [data?.metadata?.columns, handleFilter]);

  const loadData = useCallback(
    async (link?: Link) => {

      try {
        const response = link 
          ? await __request<PaginatedResponse<T>>(link)
          : await fetchData(
              undefined, 
              pageSize, 
              activeFilter?.column, 
              activeFilter?.value
            );
        setData(response);
      } catch (error) {
        console.error("Failed to load data:", error);
      }
    },
    [fetchData, activeFilter, pageSize]
  );

  useEffect(() => {
    const initialLoad = async () => {
      // Add delay only on initial load
      if (!data) {
        await new Promise(resolve => setTimeout(resolve, 2000)); // i spend a lot of time on that loader, you're looking at it!
      }
      loadData();
    };
    
    initialLoad();
  }, [loadData, activeFilter]);

  return (
    <DataTable>
      <DataTable.Content 
        data={data} 
        onFilter={handleFilter}
      >
        <DataTable.Loader />
        <DataTable.Toolbar onSearch={handleSearch}/>
        <DataTable.Container>
          <DataTable.Header />
          <DataTable.Body />
        </DataTable.Container>
      </DataTable.Content>
      <DataTable.Pagination onPaginate={(link?: Link) => loadData(link)} />
    </DataTable>
  );
}
