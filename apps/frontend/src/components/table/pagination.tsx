import React from "react";
import { Button } from "@/components/ui/button"

interface PaginationProps {
  nextPage?: () => void;
  prevPage?: () => void;
}
  

const Pagination = ({ 
  nextPage,
  prevPage,
} : PaginationProps) => {

  return (
    <div className="flex flex-initial items-center justify-end space-x-2 py-4 px-8 border-t">
      <Button
        variant="outline"
        size="sm"
        onClick={prevPage}
        disabled={!prevPage}
      >
        GLOBAL_PREV
      </Button>
      <Button
        variant="outline"
        size="sm"
        onClick={nextPage}
        disabled={!nextPage}
      >
        GLOBAL_NEXT
      </Button>
    </div>
  );
};

export default Pagination;
