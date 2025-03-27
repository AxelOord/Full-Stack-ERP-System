import { Preview } from '@storybook/react';
import "../src/app/globals.css";
import localFont from "next/font/local";
const mavenPro = localFont({ src: '../src/app/fonts/MavenPro.ttf' });

const preview: Preview = {
    parameters: {
        nextjs: {
            appDirectory: true,
        },
    },
};

export default preview;