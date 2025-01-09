"use client";

import { useEffect, useState, useCallback } from "react";
import { DataTable } from "@/components/ui/data-table";
import { Response } from "@/services";
import { requestFromUrl as __request } from '@/services/core/request';

interface TableWrapperProps<T> {
  fetchInitialData: () => Promise<Response<T>>;
  searchByTerm?: (searchTerm: string) => Promise<Response<T>>;
}

export default function TableWrapper<T extends object>({
  fetchInitialData,
  searchByTerm
}: TableWrapperProps<T>) {
  const [data, setData] = useState<Response<T> | null>(null);

  const fetchData = useCallback(
    async (link?: string | null) => {
      const response = link ? await __request<Response<T>>(link) : await fetchInitialData();
      setData(response);
    },
    [fetchInitialData]
  );

  useEffect(() => {
    fetchData();
  }, [fetchData]);

  return (
    <DataTable<T>
      result={data}
      hasNextPage={!!data?.links?.next}
      hasPrevPage={!!data?.links?.prev}
      onNextPage={() => fetchData(data?.links?.next)}
      onPrevPage={() => fetchData(data?.links?.prev)}
      onInputChange={async (searchTerm: string) => {
        if (searchByTerm) {
          const response = await searchByTerm(searchTerm);
          setData(response);
        }
      }}
    />
  );
}