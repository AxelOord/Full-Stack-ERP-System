import React from "react";
import { Button } from "./button"

interface PaginationProps {
    hasNextPage: boolean;
    hasPrevPage: boolean;
    onNextPage: () => void;
    onPrevPage: () => void;
}
  

const Pagination = ({ hasPrevPage, hasNextPage, onPrevPage, onNextPage } : PaginationProps) => {
  return (
    <div className="flex flex-initial items-center justify-end space-x-2 py-4 px-8 border-t">
      <Button
        variant="outline"
        size="sm"
        onClick={onPrevPage}
        disabled={!hasPrevPage}
      >
        GLOBAL_PREV
      </Button>
      <Button
        variant="outline"
        size="sm"
        onClick={onNextPage}
        disabled={!hasNextPage}
      >
        GLOBAL_NEXT
      </Button>
    </div>
  );
};

export default Pagination;
