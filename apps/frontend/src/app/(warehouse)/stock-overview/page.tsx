"use client"
import TableWrapper from "@/components/shared/table-wrapper";
import { ArticleDto, ArticlesService, Response } from "@/services";
import { useRefresh } from "@/context/refresh-context";

const fetchInitialArticles = async (): Promise<Response<ArticleDto>> => {
  // Create a delay function
  //const delay = (ms: number) => new Promise(resolve => setTimeout(resolve, ms));

  // Wait for the delay
  //await delay(3000);

  // Now fetch the articles
  console.log("fetching articles");
  return await ArticlesService.getApiArticles1(1, 25);
};

const searchArticles = async (searchTerm: string): Promise<Response<ArticleDto>> => {
  return await ArticlesService.getApiArticles1(undefined, 25, "articleNumber", searchTerm);
};

export default function Page() {
  const { refreshKey } = useRefresh();

  return (
    <>
      <h1 className="font-bold text-lg flex-initial px-6">
        Alle artikelen
      </h1>

      <TableWrapper<ArticleDto>
        fetchInitialData={fetchInitialArticles}
        searchByTerm={searchArticles}
        key={refreshKey}
      />
    </>
  )
}