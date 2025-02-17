/**
 * Removes duplicate items from an array based on a given key extractor function.
 * @param items The array of items to deduplicate.
 * @param keyExtractor A function that extracts the key used to determine uniqueness.
 * @returns A deduplicated array.
 */
export const deduplicate = <T>(items: T[], keyExtractor: (item: T) => string): T[] => {
    const seen = new Set<string>();
    return items.filter(item => {
        const key = keyExtractor(item);
        if (seen.has(key)) return false;
        seen.add(key);
        return true;
    });
};
