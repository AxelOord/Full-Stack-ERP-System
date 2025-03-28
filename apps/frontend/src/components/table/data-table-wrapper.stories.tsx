import React, { Suspense } from 'react';
import TableWrapper from '@/components/shared/table-wrapper';
import type { Meta, StoryObj } from '@storybook/react';
import {
  ArticleDto,
  ArticlesService,
  CancelablePromise,
  SupplierDto,
  SuppliersService,
} from '@/services';

function cancelableToPromise<T>(cancelable: CancelablePromise<T>): Promise<T> {
  return new Promise((resolve, reject) => {
    cancelable.then(resolve).catch(reject);
  });
}

type EntityKey = "article" | "supplier";

const entityConfig = {
  article: {
    label: "Article",
    fetchData: (...args: Parameters<typeof ArticlesService.getApiArticles>) =>
      cancelableToPromise(ArticlesService.getApiArticles(...args)),
    component: () => (
      <TableWrapper<ArticleDto>
        fetchData={(...args) =>
          cancelableToPromise(ArticlesService.getApiArticles(...args))
        }
      />
    ),
  },
  supplier: {
    label: "Supplier",
    fetchData: (...args: Parameters<typeof SuppliersService.getApiSuppliers>) =>
        cancelableToPromise(SuppliersService.getApiSuppliers(...args)),
    component: () => (
      <TableWrapper<SupplierDto>
        fetchData={(...args) =>
          cancelableToPromise(SuppliersService.getApiSuppliers(...args))
        }
      />
    ),
  },
};


const meta: Meta = {
    parameters: {
      controls: {
        expanded: true,
      },
    },
    argTypes: {
      entity: {
        control: { type: "select" },
        options: ["article", "supplier"],
      },
    },
  };

export default meta;

type Story = StoryObj<{
  entity: EntityKey;
}>;

export const Default: Story = {
    args: {
      entity: "article",
    },
    render: ({ entity }) => {
      const SelectedComponent = entityConfig[entity].component;
      return <Suspense>{<SelectedComponent />}</Suspense>;
    },
  };
