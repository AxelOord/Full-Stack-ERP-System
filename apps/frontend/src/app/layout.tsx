import type { Metadata } from "next";
import localFont from "next/font/local";
import "./globals.css";

import { RefreshProvider, useRefresh } from "@/context/refresh-context";
import { AppSidebar } from "@/components/sidebar/app-sidebar"
import {
  Breadcrumb,
  BreadcrumbItem,
  BreadcrumbLink,
  BreadcrumbList,
  BreadcrumbPage,
  BreadcrumbSeparator,
} from "@/components/ui/breadcrumb"
import { Separator } from "@/components/ui/separator"
import {
  SidebarInset,
  SidebarProvider,
  SidebarTrigger,
} from "@/components/ui/sidebar"

const mavenProFont = localFont({
  src: "./fonts/MavenPro.ttf",
  variable: "--font-maven-pro",
  weight: "100 900",
});
const sofiaProFont = localFont({
  src: "./fonts/Sofia Pro/Sofia Pro Regular Az.woff",
  variable: "--font-sofia-pro",
  weight: "100 900",
});

export const metadata: Metadata = {
  title: "Jeeo v2.0",
  description: "The next generation of Jeeo",
};

export default function RootLayout({
  children,
}: Readonly<{
  children: React.ReactNode;
}>) {
  return (
    <html lang="en">
      <body
        className={`${mavenProFont.variable} ${sofiaProFont.variable} antialiased`}
      >
        <SidebarProvider>
          <RefreshProvider>
            <AppSidebar/>
            <SidebarInset className="">
              <header className="flex h-16 shrink-0 items-center gap-2">
                <div className="flex items-center gap-2 px-4">
                  <SidebarTrigger className="-ml-1" />
                  <Separator orientation="vertical" className="mr-2 h-4" />
                  <Breadcrumb>
                    <BreadcrumbList>
                      <BreadcrumbItem className="hidden md:block">
                        <BreadcrumbLink href="#">
                          Magazijn
                        </BreadcrumbLink>
                      </BreadcrumbItem>
                      <BreadcrumbSeparator className="hidden md:block" />
                      <BreadcrumbItem>
                        <BreadcrumbPage>Voorraad overzicht</BreadcrumbPage>
                      </BreadcrumbItem>
                    </BreadcrumbList>
                  </Breadcrumb>
                </div>
              </header>
              <div className="flex-initial flex overflow-hidden grow">
                <div className="mx-auto w-full flex-col overflow-hidden flex-initial flex">
                  {children}
                </div>
              </div>
            </SidebarInset>
          </RefreshProvider>
        </SidebarProvider>
      </body>
    </html>
  );
}
