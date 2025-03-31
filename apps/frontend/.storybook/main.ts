import { dirname, join } from "path";
import type { StorybookConfig } from "@storybook/experimental-nextjs-vite";
import { config as loadEnv } from 'dotenv';

loadEnv({ path: join(__dirname, `.env.${process.env.STORYBOOK_ENV || 'local'}`) });

const config: StorybookConfig = {
  stories: ['../**/*.@(mdx|stories.@(js|jsx|ts|tsx))'],
  addons: [
    getAbsolutePath("@storybook/addon-essentials"),
    getAbsolutePath("@storybook/experimental-addon-test")
  ],
  framework: {
    name: getAbsolutePath("@storybook/experimental-nextjs-vite"),
    options: {},
  },
  previewAnnotations: [
    './.storybook/preview.tsx',
  ],

  viteFinal: async (config) => {
    config.define = {
      ...config.define,
      'process.env.NEXT_PUBLIC_API_BASE_URL': JSON.stringify(process.env.NEXT_PUBLIC_API_BASE_URL),
    };
    return config;
  }
};

export default config;

// To customize your webpack configuration you can use the webpackFinal field.
// Check https://storybook.js.org/docs/react/builders/webpack#extending-storybooks-webpack-config
// and https://nx.dev/recipes/storybook/custom-builder-configs

function getAbsolutePath(value: string): any {
  return dirname(require.resolve(join(value, "package.json")));
}
