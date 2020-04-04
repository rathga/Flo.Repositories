export default class ConfigProvider {
    public static get CONFIG(): { [index: string]: string } {
        return {
            apiUrl: '$VUE_APP_GOODTODO_APIURL',
        };
    }

    public static value(name: string) {
        if (!(name in this.CONFIG)) {
            return;
        }

        const value = this.CONFIG[name];

        if (!value) {
            return;
        }

        if (value.startsWith('$VUE_APP_')) {
            const envName = value.substr(1);
            const envValue = process.env[envName];
            if (envValue) {
                return envValue;
            } else {
                return
            }
        } else {
            return value;
        }
    }
}