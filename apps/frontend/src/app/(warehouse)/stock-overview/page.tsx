"use client"

import TableWrapper from "@/components/shared/table-wrapper";
import { ArticleDto, ArticlesService } from "@/services";
import { useRefresh } from "@/context/refresh-context";
import { Suspense } from "react";
import Loader from "@/components/shared/loader";

export default function Page() {
  const { refreshKey } = useRefresh();

  return (
    <>
      <h1 className="font-bold text-lg flex-initial px-6">Alle artikelen</h1>
      
      <Suspense fallback={<Loader isLoading={true} />}>
        <TableWrapper<ArticleDto>
          fetchData={ArticlesService.getApiArticles}
          key={refreshKey}
        />
      </Suspense>
    </>
  );
}
