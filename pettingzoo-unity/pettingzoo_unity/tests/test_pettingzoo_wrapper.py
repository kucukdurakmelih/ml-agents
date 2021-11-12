from pettingzoo_unity import UnityAECEnv, UnityParallelEnv
from pettingzoo_unity.tests.simple_test_envs import (
    SimpleEnvironment,
    MultiAgentEnvironment,
)
from pettingzoo.test import api_test, parallel_api_test


def test_single_agent_aec():
    unity_env = SimpleEnvironment(["test_single"])
    env = UnityAECEnv(unity_env)
    api_test(env, num_cycles=10, verbose_progress=False)


def test_multi_agent_aec():
    unity_env = MultiAgentEnvironment(["test_multi_1", "test_multi_2"], num_agents=2)
    env = UnityAECEnv(unity_env)
    api_test(env, num_cycles=10, verbose_progress=False)


def test_multi_agent_parallel():
    unity_env = MultiAgentEnvironment(
        ["test_multi_1", "test_multi_2", "test_multi_3"], num_agents=3
    )
    env = UnityParallelEnv(unity_env)
    parallel_api_test(env, num_cycles=10)
