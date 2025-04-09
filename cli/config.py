import json

CONFIG_FILENAME = 'config.json'


def get_msal_config() -> dict:
    return config['msal']


def _load_config(filename: str = CONFIG_FILENAME) -> dict:
    with open(filename, encoding='utf-8') as f:
        return json.load(f)


config = _load_config()
