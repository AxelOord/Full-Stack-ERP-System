"use client"

import { DataTable } from "@/components/table/data-table";
import { Link, PaginatedResponse } from "@/services";
import { useState, useCallback, useEffect } from "react";
import { requestFromUrl as __request } from "@/services/core/request";
import { useDataTableFilters } from "@/hooks/use-data-table-filters";

interface TableWrapperProps<T> {
  fetchData: (
    pageNumber?: number,
    pageSize?: number,
    sortBy?: string,
    isDescending?: boolean,
    rawFilters?: Record<string, string>,
  ) => Promise<PaginatedResponse<T>>;
  pageSize?: number;
}

export default function TableWrapper<T extends object>({
  fetchData,
  pageSize = 25,
}: TableWrapperProps<T>) {
  const [data, setData] = useState<PaginatedResponse<T> | null>(null);
  const [currentFilters, setCurrentFilters] = useState<Record<string, string>>({});

  const { getActiveFilters } = useDataTableFilters();

  // the filter functionality should be refactored when openapi v3.3 is released
  // then a proper deep object filter can be used ( hopefully )
  // https://github.com/OAI/OpenAPI-Specification/issues/1706#issuecomment-2704374644
  const convertFiltersToRawFilters = useCallback((filters: Record<string, { operator: string; value: string }[]>) => {
    const rawFilters: Record<string, string> = {};
    
    Object.entries(filters).forEach(([column, operations]) => {
      operations.forEach(({ operator, value }) => {
        rawFilters[`${column}[${operator}]`] = value;
      });
    });
    
    return rawFilters;
  }, []);

  const handleFilter = useCallback((activeFilters: Record<string, { operator: string; value: string }[]>) => {
    const rawFilters = convertFiltersToRawFilters(activeFilters);
    const filtersChanged = JSON.stringify(rawFilters) !== JSON.stringify(currentFilters);
    
    if (filtersChanged) {
      setCurrentFilters(rawFilters);
      return fetchData(undefined, pageSize, undefined, undefined, rawFilters).then(setData);
    }
    
    return Promise.resolve(data || null);
  }, [fetchData, pageSize, convertFiltersToRawFilters, currentFilters, data]);

  // TODO: this should be cleaner with less hardcoded values
  // This is a temporary solution to handle search
  // this should come from the backend
  const handleSearch = useCallback((searchTerm: string) => {
    const field = data?.metadata?.columns?.at(1)?.field;
    if (field && searchTerm) {
      const searchFilter = {
        [field]: [{ operator: "contains", value: searchTerm }]
      };
      return handleFilter(searchFilter);
    }
    return Promise.resolve();
  }, [data?.metadata?.columns, handleFilter]);

  const loadData = useCallback(
    async (link?: Link) => {
      try {
        const filtersToUse = !data ? 
          convertFiltersToRawFilters(getActiveFilters()) : 
          currentFilters;
        
        const response = link 
          ? await __request<PaginatedResponse<T>>(link)
          : await fetchData(
              undefined, 
              pageSize, 
              undefined, 
              undefined,
              Object.keys(filtersToUse).length > 0 ? filtersToUse : undefined
            );
        
        if (!data && Object.keys(filtersToUse).length > 0) {
          setCurrentFilters(filtersToUse);
        }
        
        setData(response);
      } catch (error) {
        console.error("Failed to load data:", error);
      }
    },
    [fetchData, currentFilters, pageSize, data, getActiveFilters, convertFiltersToRawFilters]
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
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  return (
      <DataTable>
        <DataTable.Content 
          data={data} 
          onFilter={handleFilter}
        >
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