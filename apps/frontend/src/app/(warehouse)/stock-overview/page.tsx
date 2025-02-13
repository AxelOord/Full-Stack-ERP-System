"use client"

import TableWrapper from "@/components/shared/table-wrapper";
import { ArticleDto, ArticlesService } from "@/services";
import { useRefresh } from "@/context/refresh-context";

export default function Page() {
  const { refreshKey } = useRefresh();

  return (
    <>
      <h1 className="font-bold text-lg flex-initial px-6">Alle artikelen</h1>

      <TableWrapper<ArticleDto>
        fetchData={ArticlesService.getApiArticles}
        searchByProperty="articleNumber"
        key={refreshKey}
      />
    </>
  );
}
