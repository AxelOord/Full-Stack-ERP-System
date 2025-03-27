import React from "react";
import { Suspense } from "react";
import TableWrapper from "@/components/shared/table-wrapper";
import type { Meta, StoryObj } from "@storybook/react";
import { ArticleDto, ArticlesService } from "@/services";

const meta: Meta<typeof TableWrapper<ArticleDto>> = {
    component: TableWrapper,
};

export default meta;

type Story = StoryObj<typeof TableWrapper<ArticleDto>>;

export const Default: Story = {
    render: () => {
        return (
            <Suspense>
                <TableWrapper<ArticleDto> fetchData={ArticlesService.getApiArticles} />
            </Suspense>
        );
    },
};