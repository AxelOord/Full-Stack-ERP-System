// useTestHook.ts
import { useSearchParams, useRouter, usePathname } from 'next/navigation';
import { useState, useEffect, useMemo } from 'react';

export function useTestHook() {
    const searchParams = useSearchParams();
    const router = useRouter();
    const pathname = usePathname();

    const [count, setCount] = useState(() => {
        const initial = parseInt(searchParams.get('count') || '0', 10);
        return isNaN(initial) ? 0 : initial;
    });

    const getCount = () => count;

    useEffect(() => {
        const param = parseInt(searchParams.get('count') || '0', 10);
        if (!isNaN(param) && param !== count) {
            setCount(param);
        }
    }, [searchParams]);

    const updateCount = (newCount: number) => {
        const params = new URLSearchParams(searchParams.toString());
        params.set('count', newCount.toString());
        router.push(`${pathname}?${params.toString()}`);
        setCount(newCount);
    };

    return useMemo(
        () => ({
            count, getCount, updateCount
        }),
        [
            count, getCount, updateCount
        ]
    );

}
