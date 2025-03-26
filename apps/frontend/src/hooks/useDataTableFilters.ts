import { useRouter, useSearchParams } from "next/navigation";
import { useCallback, useEffect, useMemo } from "react";
import { FilterOperator, FILTER_OPERATORS } from "@/components/table/data-table-column-header";

export function useDataTableFilters(onFiltersChange?: (activeFilters: Record<string, { operator: string; value: string }[]>) => void) {
  const router = useRouter();
  const searchParams = useSearchParams();

  const pushUrlChange = useCallback((params: URLSearchParams): void => {
    router.push(`?${params.toString()}`);
  }, [router]);

  const applyFilter = useCallback((field: string, value: string, operator: FilterOperator): void => {
    const params = new URLSearchParams(searchParams.toString());

    Object.values(FILTER_OPERATORS).forEach(op => {
      params.delete(`${field}[${op}]`);
    });

    params.set(`${field}[${operator}]`, value);
    pushUrlChange(params);
  }, [searchParams, pushUrlChange]);

  const getFilterValue = useCallback((field: string, operator: FilterOperator): string | null => {
    return searchParams.get(`${field}[${operator}]`);
  }, [searchParams]);

  const changeOperator = useCallback((field: string, oldOperator: string, newOperator: string, value: string): void => {
    const paramEntries = Array.from(searchParams.entries());
    const oldParamKey = `${field}[${oldOperator}]`;
    const newParamKey = `${field}[${newOperator}]`;
    
    const newParams = new URLSearchParams();
    let replaced = false;
    
    for (const [key, paramValue] of paramEntries) {
      if (key === oldParamKey) {
        newParams.append(newParamKey, value);
        replaced = true;
      } else {
        newParams.append(key, paramValue);
      }
    }
    
    // If the old param didn't exist, add the new one at the end
    if (!replaced) {
      newParams.append(newParamKey, value);
    }
    
    pushUrlChange(newParams);
  }, [searchParams, pushUrlChange]);

  const updateFilterValue = useCallback((field: string, operator: string, newValue: string): void => {
    // Create a params object without modifying the original structure
    const paramEntries = Array.from(searchParams.entries());
    const paramKey = `${field}[${operator}]`;
    
    // Find and update the value while maintaining position
    const newParams = new URLSearchParams();
    let updated = false;
    
    for (const [key, value] of paramEntries) {
      if (key === paramKey) {
        newParams.append(key, newValue);
        updated = true;
      } else {
        newParams.append(key, value);
      }
    }
    
    // If the param didn't exist, add it at the end
    if (!updated) {
      newParams.append(paramKey, newValue);
    }
    
    pushUrlChange(newParams);
  }, [searchParams, pushUrlChange]);

  const parseCommand = useCallback((commandString: string): { key: string; value: string; raw: string } => {
    const [key, value] = commandString.split(':');
    return {
      key,
      value,
      raw: commandString
    };
  }, []);

  const removeCommand = useCallback((command: string): void => {
    const { key } = parseCommand(command);
    const params = new URLSearchParams(searchParams.toString());
    params.delete(key);
    pushUrlChange(params);
  }, [searchParams, parseCommand, pushUrlChange]);

  const clearAllFilters = useCallback((): void => {
    const params = new URLSearchParams(searchParams.toString());
    const excludedParams = ['page', 'size', 'q'];
    
    Array.from(params.keys()).forEach(key => {
      if (!excludedParams.includes(key)) {
        params.delete(key);
      }
    });
    
    pushUrlChange(params);
  }, [searchParams, pushUrlChange]);

  const getActiveFilters = useCallback((): Record<string, { operator: string; value: string }[]> => {
    const filters: Record<string, { operator: string; value: string }[]> = {};
    
    Array.from(searchParams.entries()).forEach(([key, value]) => {
      const match = key.match(/^([^\[]+)\[([^\]]+)\]$/);
      if (match) {
        const [, field, operator] = match;
        if (!filters[field]) {
          filters[field] = [];
        }
        filters[field].push({ operator, value });
      }
    });
    
    return filters;
  }, [searchParams]);

  useEffect(() => {
    if (onFiltersChange) {
      const activeFilters = getActiveFilters();
      onFiltersChange(activeFilters);
    }
  }, [searchParams, onFiltersChange, getActiveFilters]);

  return useMemo(() => ({
    applyFilter,
    getFilterValue,
    changeOperator,
    updateFilterValue,
    removeCommand,
    clearAllFilters,
    getActiveFilters,
    parseCommand,
    searchParams
  }), [
    applyFilter,
    getFilterValue,
    changeOperator,
    updateFilterValue,
    removeCommand,
    clearAllFilters,
    getActiveFilters,
    parseCommand,
    searchParams
  ]);
}